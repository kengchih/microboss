using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroBoss.Web.Controllers;

[ApiController]
[Route("api/inventory")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<InventoryDto>>> Query(
        [FromQuery] string? keyword,
        [FromQuery] string? stockNo,
        [FromQuery] bool? underSafeQty,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new InventoryQueryDto
        {
            Keyword = keyword,
            StockNo = stockNo,
            UnderSafeQty = underSafeQty,
            Page = page,
            PageSize = pageSize
        };
        return Ok(await _inventoryService.QueryAsync(query));
    }

    [HttpGet("under-safe-qty")]
    public async Task<ActionResult<List<InventoryDto>>> GetUnderSafeQty()
    {
        return Ok(await _inventoryService.GetUnderSafeQtyAsync());
    }

    [HttpPut("{productId}/{stockNo}")]
    public async Task<IActionResult> UpdateStock(
        string productId,
        string stockNo,
        [FromBody] UpdateStockRequest request)
    {
        await _inventoryService.UpdateStockAsync(productId, stockNo, request.Qty, request.SafeQty, request.Location);
        return NoContent();
    }

    [HttpPost("adjust")]
    public async Task<IActionResult> AdjustStock([FromBody] AdjustStockRequest request)
    {
        var operatorId = User.Identity?.Name ?? "system";
        await _inventoryService.AdjustStockAsync(request.ProductId, request.ActionType, request.Qty, request.RefNo, operatorId);
        return NoContent();
    }

    [HttpGet("maintenance-logs")]
    public async Task<ActionResult<List<MaintenanceLogDto>>> GetMaintenanceLogs()
    {
        return Ok(await _inventoryService.GetMaintenanceLogsAsync());
    }

    [HttpDelete("maintenance-logs/{logId}")]
    public async Task<IActionResult> DeleteMaintenanceLog(string logId)
    {
        await _inventoryService.DeleteMaintenanceLogAsync(logId);
        return NoContent();
    }
}

public record UpdateStockRequest(int Qty, int? SafeQty, string? Location);
public record AdjustStockRequest(string ProductId, int ActionType, int Qty, string? RefNo);
