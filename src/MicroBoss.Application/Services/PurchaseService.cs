using MapsterMapper;
using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using MicroBoss.Domain.Entities;
using MicroBoss.Domain.Enums;
using MicroBoss.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroBoss.Application.Services;

public class PurchaseService : IPurchaseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISequenceGenerator _sequenceGenerator;

    public PurchaseService(IUnitOfWork unitOfWork, IMapper mapper, ISequenceGenerator sequenceGenerator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sequenceGenerator = sequenceGenerator;
    }

    public async Task<PagedResult<PurchaseDto>> QueryAsync(PurchaseQueryDto query)
    {
        IQueryable<Purchase> q = _unitOfWork.Repository<Purchase>().Query()
            .Include(p => p.PurchaseDetails);

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            var kw = query.Keyword.Trim();
            q = q.Where(p =>
                p.PurchaseId.Contains(kw) ||
                (p.SupplierShortName != null && p.SupplierShortName.Contains(kw)));
        }

        if (query.Status.HasValue)
            q = q.Where(p => p.Status == query.Status.Value);

        if (query.DateFrom.HasValue)
            q = q.Where(p => p.PurchaseDate >= query.DateFrom.Value);

        if (query.DateTo.HasValue)
            q = q.Where(p => p.PurchaseDate <= query.DateTo.Value);

        var totalCount = await q.CountAsync();

        var items = await q
            .OrderByDescending(p => p.CreateTime)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResult<PurchaseDto>
        {
            Items = _mapper.Map<List<PurchaseDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<PurchaseDto?> GetByIdAsync(string purchaseId)
    {
        var purchase = await _unitOfWork.Repository<Purchase>().Query()
            .Include(p => p.PurchaseDetails)
            .FirstOrDefaultAsync(p => p.PurchaseId == purchaseId);

        return purchase == null ? null : _mapper.Map<PurchaseDto>(purchase);
    }

    public async Task<PurchaseDto> CreateAsync(CreatePurchaseDto dto, string operatorId)
    {
        var purchaseId = await _sequenceGenerator.GetNextPurchaseNoAsync();
        var taxType = (TaxType)dto.TaxType;
        var subTotal = dto.Details.Sum(d => (d.ItemPrice ?? 0) * (d.Qty ?? 0));

        var (taxRate, taxAmount) = CalculateTax(taxType, subTotal);

        var purchase = new Purchase
        {
            PurchaseId = purchaseId,
            SupplierId = dto.SupplierId,
            PurchaseDate = dto.PurchaseDate,
            TaxType = taxType,
            InvoiceType = (InvoiceType)dto.InvoiceType,
            PurchaseNote = dto.PurchaseNote,
            TaxRate = taxRate,
            TaxAmount = taxAmount,
            Status = PurchaseStatus.Pending,
            CreateTime = DateTime.Now,
            CreatedOperator = operatorId
        };

        int sortIndex = 1;
        foreach (var item in dto.Details)
        {
            var itemSubTotal = (item.ItemPrice ?? 0) * (item.Qty ?? 0);
            purchase.PurchaseDetails.Add(new PurchaseDetail
            {
                PurchaseId = purchaseId,
                ItemId = Guid.NewGuid().ToString("N")[..8].ToUpper(),
                ProductId = item.ProductId,
                ProductNo = item.ProductNo,
                ProductChtName = item.ProductChtName,
                ItemPrice = item.ItemPrice,
                Qty = item.Qty,
                SubTotal = itemSubTotal,
                StockNo = item.StockNo,
                ItemNote = item.ItemNote,
                ItemDiscount = 0,
                SortIndex = sortIndex++,
                ItemStatus = PurchaseItemStatus.Pending
            });
        }

        _unitOfWork.Repository<Purchase>().Add(purchase);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PurchaseDto>(purchase);
    }

    public async Task UpdateAsync(string purchaseId, CreatePurchaseDto dto)
    {
        var purchase = await _unitOfWork.Repository<Purchase>().Query()
            .Include(p => p.PurchaseDetails)
            .FirstOrDefaultAsync(p => p.PurchaseId == purchaseId)
            ?? throw new InvalidOperationException($"採購單 {purchaseId} 不存在");

        var taxType = (TaxType)dto.TaxType;
        var subTotal = dto.Details.Sum(d => (d.ItemPrice ?? 0) * (d.Qty ?? 0));
        var (taxRate, taxAmount) = CalculateTax(taxType, subTotal);

        purchase.SupplierId = dto.SupplierId;
        purchase.PurchaseDate = dto.PurchaseDate;
        purchase.TaxType = taxType;
        purchase.InvoiceType = (InvoiceType)dto.InvoiceType;
        purchase.PurchaseNote = dto.PurchaseNote;
        purchase.TaxRate = taxRate;
        purchase.TaxAmount = taxAmount;

        // Replace details
        foreach (var detail in purchase.PurchaseDetails.ToList())
            _unitOfWork.Repository<PurchaseDetail>().Remove(detail);

        purchase.PurchaseDetails.Clear();

        int sortIndex = 1;
        foreach (var item in dto.Details)
        {
            var itemSubTotal = (item.ItemPrice ?? 0) * (item.Qty ?? 0);
            purchase.PurchaseDetails.Add(new PurchaseDetail
            {
                PurchaseId = purchaseId,
                ItemId = Guid.NewGuid().ToString("N")[..8].ToUpper(),
                ProductId = item.ProductId,
                ProductNo = item.ProductNo,
                ProductChtName = item.ProductChtName,
                ItemPrice = item.ItemPrice,
                Qty = item.Qty,
                SubTotal = itemSubTotal,
                StockNo = item.StockNo,
                ItemNote = item.ItemNote,
                ItemDiscount = 0,
                SortIndex = sortIndex++,
                ItemStatus = PurchaseItemStatus.Pending
            });
        }

        _unitOfWork.Repository<Purchase>().Update(purchase);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(string purchaseId)
    {
        var purchase = await _unitOfWork.Repository<Purchase>().Query()
            .Include(p => p.PurchaseDetails)
            .FirstOrDefaultAsync(p => p.PurchaseId == purchaseId)
            ?? throw new InvalidOperationException($"採購單 {purchaseId} 不存在");

        foreach (var detail in purchase.PurchaseDetails.ToList())
            _unitOfWork.Repository<PurchaseDetail>().Remove(detail);

        _unitOfWork.Repository<Purchase>().Remove(purchase);
        await _unitOfWork.SaveChangesAsync();
    }

    private static (decimal taxRate, decimal taxAmount) CalculateTax(TaxType taxType, decimal subTotal)
    {
        return taxType switch
        {
            TaxType.None => (0m, 0m),
            TaxType.Include => (0.05m, subTotal - subTotal / 1.05m),
            TaxType.Exclude => (0.05m, subTotal * 0.05m),
            _ => (0m, 0m)
        };
    }
}
