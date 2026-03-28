using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;

namespace MicroBoss.Application.Interfaces;

public interface ICustomerService
{
    Task<PagedResult<CustomerDto>> QueryAsync(CustomerQueryDto query);
    Task<CustomerDto?> GetByIdAsync(string id);
    Task CreateAsync(CreateCustomerDto dto);
    Task UpdateAsync(string id, CreateCustomerDto dto);
    Task DeleteAsync(string id);
}
