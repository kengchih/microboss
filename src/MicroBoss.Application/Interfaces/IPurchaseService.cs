using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;

namespace MicroBoss.Application.Interfaces;

public interface IPurchaseService
{
    Task<PagedResult<PurchaseDto>> QueryAsync(PurchaseQueryDto query);
    Task<PurchaseDto?> GetByIdAsync(string purchaseId);
    Task<PurchaseDto> CreateAsync(CreatePurchaseDto dto, string operatorId);
    Task UpdateAsync(string purchaseId, CreatePurchaseDto dto);
    Task DeleteAsync(string purchaseId);
}
