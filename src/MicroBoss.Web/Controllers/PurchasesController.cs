using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using MicroBoss.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroBoss.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurchasesController : ControllerBase
{
    private readonly IPurchaseService _purchaseService;

    public PurchasesController(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<PurchaseDto>>> Query(
        [FromQuery] string? keyword,
        [FromQuery] PurchaseStatus? status,
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new PurchaseQueryDto
        {
            Keyword = keyword,
            Status = status,
            DateFrom = dateFrom,
            DateTo = dateTo,
            Page = page,
            PageSize = pageSize
        };
        return Ok(await _purchaseService.QueryAsync(query));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PurchaseDto>> GetById(string id)
    {
        var purchase = await _purchaseService.GetByIdAsync(id);
        return purchase == null ? NotFound() : Ok(purchase);
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseDto>> Create(CreatePurchaseDto dto)
    {
        var operatorId = User.Identity?.Name ?? "system";
        var result = await _purchaseService.CreateAsync(dto, operatorId);
        return CreatedAtAction(nameof(GetById), new { id = result.PurchaseId }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, CreatePurchaseDto dto)
    {
        await _purchaseService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _purchaseService.DeleteAsync(id);
        return NoContent();
    }
}
