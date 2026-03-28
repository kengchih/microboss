using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;

namespace MicroBoss.Application.Interfaces;

public interface ISupplierService
{
    Task<PagedResult<SupplierDto>> QueryAsync(SupplierQueryDto query);
    Task<SupplierDto?> GetByIdAsync(string id);
    Task<SupplierDto> CreateAsync(CreateSupplierDto dto);
    Task UpdateAsync(string id, CreateSupplierDto dto);
    Task DeleteAsync(string id);
}
