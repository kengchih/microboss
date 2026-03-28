using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroBoss.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<SupplierDto>>> Query([FromQuery] SupplierQueryDto query)
        => Ok(await _supplierService.QueryAsync(query));

    [HttpGet("{id}")]
    public async Task<ActionResult<SupplierDto>> GetById(string id)
    {
        var supplier = await _supplierService.GetByIdAsync(id);
        return supplier == null ? NotFound() : Ok(supplier);
    }

    [HttpPost]
    public async Task<ActionResult<SupplierDto>> Create([FromBody] CreateSupplierDto dto)
    {
        var result = await _supplierService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.SupplierId }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] CreateSupplierDto dto)
    {
        await _supplierService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _supplierService.DeleteAsync(id);
        return NoContent();
    }
}
