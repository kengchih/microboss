using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;

namespace MicroBoss.Application.Interfaces;

public interface IOrderService
{
    Task<PagedResult<OrderDto>> QueryAsync(OrderQueryDto query);
    Task<OrderDto?> GetByIdAsync(string orderNo);
    Task<OrderDto> CreateAsync(CreateOrderDto dto, string operatorId);
    Task UpdateAsync(string orderNo, CreateOrderDto dto);
    Task DeleteAsync(string orderNo);
    Task ConfirmAsync(string orderNo);
    Task InvalidateAsync(string orderNo);
}
