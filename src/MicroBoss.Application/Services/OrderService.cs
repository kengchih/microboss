using MapsterMapper;
using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using MicroBoss.Domain.Entities;
using MicroBoss.Domain.Enums;
using MicroBoss.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroBoss.Application.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly ISequenceGenerator _sequenceGenerator;

    public OrderService(IUnitOfWork uow, IMapper mapper, ISequenceGenerator sequenceGenerator)
    {
        _uow = uow;
        _mapper = mapper;
        _sequenceGenerator = sequenceGenerator;
    }

    public async Task<PagedResult<OrderDto>> QueryAsync(OrderQueryDto query)
    {
        var q = _uow.Repository<Order>().Query()
            .Include(o => o.OrderDetails);

        IQueryable<Order> filtered = q;

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            var kw = query.Keyword.Trim();
            filtered = filtered.Where(o =>
                (o.OrderNo.Contains(kw)) ||
                (o.CustomerName != null && o.CustomerName.Contains(kw)) ||
                (o.CustomerId != null && o.CustomerId.Contains(kw)));
        }

        if (query.Status.HasValue)
            filtered = filtered.Where(o => o.Status == query.Status.Value);

        if (query.DateFrom.HasValue)
            filtered = filtered.Where(o => o.OrderDate >= query.DateFrom.Value);

        if (query.DateTo.HasValue)
            filtered = filtered.Where(o => o.OrderDate <= query.DateTo.Value);

        if (!string.IsNullOrWhiteSpace(query.CustomerId))
            filtered = filtered.Where(o => o.CustomerId == query.CustomerId);

        var totalCount = await filtered.CountAsync();

        var items = await filtered
            .OrderByDescending(o => o.CreateTime)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResult<OrderDto>
        {
            Items = _mapper.Map<List<OrderDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<OrderDto?> GetByIdAsync(string orderNo)
    {
        var order = await _uow.Repository<Order>().Query()
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.OrderNo == orderNo);

        return order == null ? null : _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> CreateAsync(CreateOrderDto dto, string operatorId)
    {
        var orderNo = await _sequenceGenerator.GetNextOrderNoAsync();

        var subTotal = dto.OrderDetails.Sum(d => d.OrderQty * d.ItemPrice - d.ItemDiscount);

        decimal taxRate = 0m;
        decimal taxAmount = 0m;
        var taxType = dto.TaxType.HasValue ? (TaxType)dto.TaxType.Value : TaxType.None;
        switch (taxType)
        {
            case TaxType.Include:
                taxRate = 0.05m;
                taxAmount = Math.Round(subTotal - subTotal / 1.05m, 2);
                break;
            case TaxType.Exclude:
                taxRate = 0.05m;
                taxAmount = Math.Round(subTotal * 0.05m, 2);
                break;
            default:
                taxRate = 0m;
                taxAmount = 0m;
                break;
        }

        var actualAmount = taxType == TaxType.Exclude
            ? subTotal + taxAmount
            : subTotal;

        var order = new Order
        {
            OrderNo = orderNo,
            CustomerId = dto.CustomerId,
            CustomerName = dto.CustomerName,
            OrderDate = dto.OrderDate,
            TaxType = taxType,
            InvoiceType = dto.InvoiceType.HasValue ? (InvoiceType)dto.InvoiceType.Value : null,
            DeliveryDate = dto.DeliveryDate,
            PaymentMethod = dto.PaymentMethod,
            ContactWindow = dto.ContactWindow,
            DeliveryAddressZip = dto.DeliveryAddressZip,
            DeliveryAddress = dto.DeliveryAddress,
            DeliveryMethod = dto.DeliveryMethod,
            UniteNo = dto.UniteNo,
            InvoiceTitle = dto.InvoiceTitle,
            OrderNote = dto.OrderNote,
            PrePayAmount = dto.PrePayAmount,
            TaxRate = taxRate,
            TaxAmount = taxAmount,
            ActualAmount = actualAmount,
            Status = OrderStatus.Pending,
            CreateTime = DateTime.Now,
            CreateOperator = operatorId
        };

        foreach (var d in dto.OrderDetails)
        {
            order.OrderDetails.Add(new OrderDetail
            {
                DetailId = Guid.NewGuid().ToString(),
                OrderNo = orderNo,
                ProductId = d.ProductId,
                ProductNo = d.ProductNo,
                ProductName = d.ProductName,
                BaseUnit = d.BaseUnit,
                OrderUnit = d.OrderUnit,
                OrderQty = d.OrderQty,
                ItemPrice = d.ItemPrice,
                ItemDiscount = d.ItemDiscount,
                ItemAmount = d.OrderQty * d.ItemPrice - d.ItemDiscount,
                ItemNote = d.ItemNote
            });
        }

        _uow.Repository<Order>().Add(order);
        await _uow.SaveChangesAsync();

        return _mapper.Map<OrderDto>(order);
    }

    public async Task UpdateAsync(string orderNo, CreateOrderDto dto)
    {
        var order = await _uow.Repository<Order>().Query()
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.OrderNo == orderNo)
            ?? throw new InvalidOperationException("訂單不存在");

        var subTotal = dto.OrderDetails.Sum(d => d.OrderQty * d.ItemPrice - d.ItemDiscount);

        decimal taxRate = 0m;
        decimal taxAmount = 0m;
        var taxType = dto.TaxType.HasValue ? (TaxType)dto.TaxType.Value : TaxType.None;
        switch (taxType)
        {
            case TaxType.Include:
                taxRate = 0.05m;
                taxAmount = Math.Round(subTotal - subTotal / 1.05m, 2);
                break;
            case TaxType.Exclude:
                taxRate = 0.05m;
                taxAmount = Math.Round(subTotal * 0.05m, 2);
                break;
            default:
                taxRate = 0m;
                taxAmount = 0m;
                break;
        }

        var actualAmount = taxType == TaxType.Exclude
            ? subTotal + taxAmount
            : subTotal;

        order.CustomerId = dto.CustomerId;
        order.CustomerName = dto.CustomerName;
        order.OrderDate = dto.OrderDate;
        order.TaxType = taxType;
        order.InvoiceType = dto.InvoiceType.HasValue ? (InvoiceType)dto.InvoiceType.Value : null;
        order.DeliveryDate = dto.DeliveryDate;
        order.PaymentMethod = dto.PaymentMethod;
        order.ContactWindow = dto.ContactWindow;
        order.DeliveryAddressZip = dto.DeliveryAddressZip;
        order.DeliveryAddress = dto.DeliveryAddress;
        order.DeliveryMethod = dto.DeliveryMethod;
        order.UniteNo = dto.UniteNo;
        order.InvoiceTitle = dto.InvoiceTitle;
        order.OrderNote = dto.OrderNote;
        order.PrePayAmount = dto.PrePayAmount;
        order.TaxRate = taxRate;
        order.TaxAmount = taxAmount;
        order.ActualAmount = actualAmount;
        order.LastUpdateTime = DateTime.Now;

        // Replace detail items
        foreach (var existing in order.OrderDetails.ToList())
            _uow.Repository<OrderDetail>().Remove(existing);

        foreach (var d in dto.OrderDetails)
        {
            order.OrderDetails.Add(new OrderDetail
            {
                DetailId = Guid.NewGuid().ToString(),
                OrderNo = orderNo,
                ProductId = d.ProductId,
                ProductNo = d.ProductNo,
                ProductName = d.ProductName,
                BaseUnit = d.BaseUnit,
                OrderUnit = d.OrderUnit,
                OrderQty = d.OrderQty,
                ItemPrice = d.ItemPrice,
                ItemDiscount = d.ItemDiscount,
                ItemAmount = d.OrderQty * d.ItemPrice - d.ItemDiscount,
                ItemNote = d.ItemNote
            });
        }

        _uow.Repository<Order>().Update(order);
        await _uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(string orderNo)
    {
        var order = await _uow.Repository<Order>().Query()
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.OrderNo == orderNo)
            ?? throw new InvalidOperationException("訂單不存在");

        foreach (var detail in order.OrderDetails.ToList())
            _uow.Repository<OrderDetail>().Remove(detail);

        _uow.Repository<Order>().Remove(order);
        await _uow.SaveChangesAsync();
    }

    public async Task ConfirmAsync(string orderNo)
    {
        var order = await _uow.Repository<Order>().Query()
            .FirstOrDefaultAsync(o => o.OrderNo == orderNo)
            ?? throw new InvalidOperationException("訂單不存在");

        order.Status = OrderStatus.UnPaid;
        order.LastUpdateTime = DateTime.Now;
        _uow.Repository<Order>().Update(order);
        await _uow.SaveChangesAsync();
    }

    public async Task InvalidateAsync(string orderNo)
    {
        var order = await _uow.Repository<Order>().Query()
            .FirstOrDefaultAsync(o => o.OrderNo == orderNo)
            ?? throw new InvalidOperationException("訂單不存在");

        order.Status = OrderStatus.Invalid;
        order.LastUpdateTime = DateTime.Now;
        _uow.Repository<Order>().Update(order);
        await _uow.SaveChangesAsync();
    }
}
