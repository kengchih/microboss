using MapsterMapper;
using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using MicroBoss.Domain.Entities;
using MicroBoss.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroBoss.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<ProductDto>> QueryAsync(ProductQueryDto query)
    {
        IQueryable<Product> q = _unitOfWork.Repository<Product>().Query()
            .Include(p => p.ProductCost)
            .Include(p => p.ProductStocks);

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            var kw = query.Keyword.Trim();
            q = q.Where(p =>
                (p.ProductName != null && p.ProductName.Contains(kw)) ||
                (p.ProductNo != null && p.ProductNo.Contains(kw)) ||
                (p.ProductId.Contains(kw)) ||
                (p.Manufacturer != null && p.Manufacturer.Contains(kw)));
        }

        if (query.ProductClass.HasValue)
            q = q.Where(p => p.ProductClass == query.ProductClass.Value);

        if (!string.IsNullOrWhiteSpace(query.MainCategory))
            q = q.Where(p => p.MainCategory == query.MainCategory);

        if (query.IsSuspend.HasValue)
            q = q.Where(p => p.IsSuspend == query.IsSuspend.Value);

        var totalCount = await q.CountAsync();

        var items = await q
            .OrderBy(p => p.ProductNo)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResult<ProductDto>
        {
            Items = _mapper.Map<List<ProductDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<ProductDto?> GetByIdAsync(string id)
    {
        var product = await _unitOfWork.Repository<Product>().Query()
            .Include(p => p.ProductCost)
            .Include(p => p.ProductStocks)
            .FirstOrDefaultAsync(p => p.ProductId == id);

        return product == null ? null : _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto?> GetByNoAsync(string productNo)
    {
        var product = await _unitOfWork.Repository<Product>().Query()
            .Include(p => p.ProductCost)
            .Include(p => p.ProductStocks)
            .FirstOrDefaultAsync(p => p.ProductNo == productNo);

        return product == null ? null : _mapper.Map<ProductDto>(product);
    }

    public async Task CreateAsync(CreateProductDto dto)
    {
        var entity = _mapper.Map<Product>(dto);
        entity.CreateTime = DateTime.Now;
        _unitOfWork.Repository<Product>().Add(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(string id, CreateProductDto dto)
    {
        var entity = await _unitOfWork.Repository<Product>().Query()
            .FirstOrDefaultAsync(p => p.ProductId == id)
            ?? throw new InvalidOperationException("產品不存在");

        entity.ProductNo = dto.ProductNo;
        entity.ProductClass = dto.ProductClass;
        entity.MainCategory = dto.MainCategory;
        entity.SubCategory = dto.SubCategory;
        entity.ProductName = dto.ProductName;
        entity.ProductNameExt = dto.ProductNameExt;
        entity.BaseUnit = dto.BaseUnit;
        entity.BaseBarcode = dto.BaseBarcode;
        entity.Manufacturer = dto.Manufacturer;
        entity.DefaultStockNo = dto.DefaultStockNo;
        entity.LastUpdateTime = DateTime.Now;

        _unitOfWork.Repository<Product>().Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await _unitOfWork.Repository<Product>().Query()
            .FirstOrDefaultAsync(p => p.ProductId == id)
            ?? throw new InvalidOperationException("產品不存在");

        _unitOfWork.Repository<Product>().Remove(entity);
        await _unitOfWork.SaveChangesAsync();
    }
}
