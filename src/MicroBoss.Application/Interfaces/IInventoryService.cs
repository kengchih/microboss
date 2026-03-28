using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;

namespace MicroBoss.Application.Interfaces;

public interface IInventoryService
{
    Task<PagedResult<InventoryDto>> QueryAsync(InventoryQueryDto query);
    Task<List<InventoryDto>> GetUnderSafeQtyAsync();
    Task UpdateStockAsync(string productId, string stockNo, int qty, int? safeQty, string? location);
    Task AdjustStockAsync(string productId, int actionType, int qty, string? refNo, string operatorId);
    Task<List<MaintenanceLogDto>> GetMaintenanceLogsAsync();
    Task DeleteMaintenanceLogAsync(string logId);
}
