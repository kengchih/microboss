using MapsterMapper;
using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using MicroBoss.Domain.Entities;
using MicroBoss.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroBoss.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISequenceGenerator _sequenceGenerator;

    public SupplierService(IUnitOfWork unitOfWork, IMapper mapper, ISequenceGenerator sequenceGenerator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sequenceGenerator = sequenceGenerator;
    }

    public async Task<PagedResult<SupplierDto>> QueryAsync(SupplierQueryDto query)
    {
        var q = _unitOfWork.Repository<Supplier>().Query();

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            var kw = query.Keyword.Trim();
            q = q.Where(s =>
                (s.SupplierFullName != null && s.SupplierFullName.Contains(kw)) ||
                (s.SupplierShortName != null && s.SupplierShortName.Contains(kw)) ||
                s.SupplierId.Contains(kw));
        }

        var totalCount = await q.CountAsync();

        var items = await q
            .OrderBy(s => s.SupplierId)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResult<SupplierDto>
        {
            Items = _mapper.Map<List<SupplierDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<SupplierDto?> GetByIdAsync(string id)
    {
        var supplier = await _unitOfWork.Repository<Supplier>()
            .Query()
            .Include(s => s.Banks)
            .FirstOrDefaultAsync(s => s.SupplierId == id);

        return supplier == null ? null : _mapper.Map<SupplierDto>(supplier);
    }

    public async Task<SupplierDto> CreateAsync(CreateSupplierDto dto)
    {
        var id = await _sequenceGenerator.GetNextSupplierIdAsync();

        var supplier = _mapper.Map<Supplier>(dto);
        supplier.SupplierId = id;
        supplier.CreateTime = DateTime.Now;

        _unitOfWork.Repository<Supplier>().Add(supplier);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<SupplierDto>(supplier);
    }

    public async Task UpdateAsync(string id, CreateSupplierDto dto)
    {
        var supplier = await _unitOfWork.Repository<Supplier>()
            .Query()
            .FirstOrDefaultAsync(s => s.SupplierId == id)
            ?? throw new InvalidOperationException($"供應商 {id} 不存在");

        supplier.SupplierShortName = dto.SupplierShortName;
        supplier.SupplierFullName = dto.SupplierFullName;
        supplier.UniteTitle = dto.UniteTitle;
        supplier.SupplierType = dto.SupplierType;
        supplier.TaxType = dto.TaxType;
        supplier.Tel1 = dto.Tel1;
        supplier.Fax = dto.Fax;
        supplier.Email = dto.Email;
        supplier.ContactWindow = dto.ContactWindow;
        supplier.RegisterAddress = dto.RegisterAddress;
        supplier.Note = dto.Note;
        supplier.LastUpdateTime = DateTime.Now;

        _unitOfWork.Repository<Supplier>().Update(supplier);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var supplier = await _unitOfWork.Repository<Supplier>()
            .Query()
            .FirstOrDefaultAsync(s => s.SupplierId == id)
            ?? throw new InvalidOperationException($"供應商 {id} 不存在");

        _unitOfWork.Repository<Supplier>().Remove(supplier);
        await _unitOfWork.SaveChangesAsync();
    }
}
