using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using MicroBoss.Domain.Entities;
using MicroBoss.Domain.Enums;
using MicroBoss.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroBoss.Application.Services;

public class InventoryService : IInventoryService
{
    private readonly IUnitOfWork _uow;

    public InventoryService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<PagedResult<InventoryDto>> QueryAsync(InventoryQueryDto query)
    {
        IQueryable<ProductStock> q = _uow.Repository<ProductStock>()
            .Query()
            .Include(ps => ps.Product);

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            var kw = query.Keyword.Trim();
            q = q.Where(ps =>
                (ps.Product.ProductNo != null && ps.Product.ProductNo.Contains(kw)) ||
                (ps.Product.ProductName != null && ps.Product.ProductName.Contains(kw)));
        }

        if (!string.IsNullOrWhiteSpace(query.StockNo))
            q = q.Where(ps => ps.StockNo == query.StockNo);

        if (query.UnderSafeQty == true)
            q = q.Where(ps => ps.StockQty < ps.SafeQty);

        var totalCount = await q.CountAsync();

        var items = await q
            .OrderBy(ps => ps.Product.ProductNo)
            .ThenBy(ps => ps.StockNo)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResult<InventoryDto>
        {
            Items = items.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<List<InventoryDto>> GetUnderSafeQtyAsync()
    {
        var items = await _uow.Repository<ProductStock>()
            .Query()
            .Include(ps => ps.Product)
            .Where(ps => ps.StockQty < ps.SafeQty)
            .OrderBy(ps => ps.Product.ProductNo)
            .ToListAsync();

        return items.Select(MapToDto).ToList();
    }

    public async Task UpdateStockAsync(string productId, string stockNo, int qty, int? safeQty, string? location)
    {
        var stock = await _uow.Repository<ProductStock>()
            .Query()
            .FirstOrDefaultAsync(ps => ps.ProductId == productId && ps.StockNo == stockNo)
            ?? throw new InvalidOperationException($"庫存記錄不存在 (ProductId={productId}, StockNo={stockNo})");

        stock.StockQty = qty;
        if (safeQty.HasValue)
            stock.SafeQty = safeQty.Value;
        if (location != null)
            stock.Location = location;
        stock.LastUpdateTime = DateTime.Now;

        _uow.Repository<ProductStock>().Update(stock);
        await _uow.SaveChangesAsync();
    }

    public async Task AdjustStockAsync(string productId, int actionType, int qty, string? refNo, string operatorId)
    {
        var stockActionType = (StockActionType)actionType;

        var stock = await _uow.Repository<ProductStock>()
            .Query()
            .Include(ps => ps.Product)
            .FirstOrDefaultAsync(ps => ps.ProductId == productId)
            ?? throw new InvalidOperationException($"庫存記錄不存在 (ProductId={productId})");

        // Adjust qty: Take/Sale reduce stock; Return/Purchase/Adjust add stock
        int delta = stockActionType switch
        {
            StockActionType.Take => -qty,
            StockActionType.Sale => -qty,
            StockActionType.Return => qty,
            StockActionType.Purchase => qty,
            StockActionType.Adjust => qty,
            _ => qty
        };

        stock.StockQty = (stock.StockQty ?? 0) + delta;
        stock.LastUpdateTime = DateTime.Now;
        _uow.Repository<ProductStock>().Update(stock);

        var log = new MaintenancePartLog
        {
            PartLogId = Guid.NewGuid().ToString("N")[..20],
            LogTime = DateTime.Now,
            LogType = stockActionType,
            ProductId = productId,
            ProductNo = stock.Product?.ProductNo,
            Barcode = stock.Product?.BaseBarcode,
            Qty = qty,
            OperatorId = operatorId,
            SettleNo = refNo
        };

        _uow.Repository<MaintenancePartLog>().Add(log);
        await _uow.SaveChangesAsync();
    }

    public async Task<List<MaintenanceLogDto>> GetMaintenanceLogsAsync()
    {
        var logs = await _uow.Repository<MaintenancePartLog>()
            .Query()
            .OrderByDescending(l => l.LogTime)
            .ToListAsync();

        return logs.Select(l => new MaintenanceLogDto
        {
            PartLogId = l.PartLogId,
            LogTime = l.LogTime,
            LogType = l.LogType,
            ProductNo = l.ProductNo,
            ProductId = l.ProductId,
            Barcode = l.Barcode,
            Qty = l.Qty,
            OperatorId = l.OperatorId,
            LogUnit = l.LogUnit
        }).ToList();
    }

    public async Task DeleteMaintenanceLogAsync(string logId)
    {
        var log = await _uow.Repository<MaintenancePartLog>()
            .Query()
            .FirstOrDefaultAsync(l => l.PartLogId == logId)
            ?? throw new InvalidOperationException($"記錄不存在 (PartLogId={logId})");

        _uow.Repository<MaintenancePartLog>().Remove(log);
        await _uow.SaveChangesAsync();
    }

    private static InventoryDto MapToDto(ProductStock ps) => new()
    {
        ProductId = ps.ProductId,
        ProductNo = ps.Product?.ProductNo,
        ProductName = ps.Product?.ProductName,
        ProductNameExt = ps.Product?.ProductNameExt,
        Manufacturer = ps.Product?.Manufacturer,
        Voltage = ps.Product?.Voltage,
        BaseBarcode = ps.Product?.BaseBarcode,
        BaseUnit = ps.Product?.BaseUnit,
        IsSuspend = ps.Product?.IsSuspend,
        StockNo = ps.StockNo,
        SafeQty = ps.SafeQty ?? 0,
        CurrentStockQty = ps.StockQty ?? 0,
        TransitQty = ps.TransitQty ?? 0,
        Location = ps.Location
    };
}
