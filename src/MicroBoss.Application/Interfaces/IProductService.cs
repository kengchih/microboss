using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;

namespace MicroBoss.Application.Interfaces;

public interface IProductService
{
    Task<PagedResult<ProductDto>> QueryAsync(ProductQueryDto query);
    Task<ProductDto?> GetByIdAsync(string id);
    Task<ProductDto?> GetByNoAsync(string productNo);
    Task CreateAsync(CreateProductDto dto);
    Task UpdateAsync(string id, CreateProductDto dto);
    Task DeleteAsync(string id);
}
