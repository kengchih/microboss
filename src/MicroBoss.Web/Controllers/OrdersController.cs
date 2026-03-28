using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroBoss.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<OrderDto>>> Query([FromQuery] OrderQueryDto query)
        => Ok(await _orderService.QueryAsync(query));

    [HttpGet("{orderNo}")]
    public async Task<ActionResult<OrderDto>> GetById(string orderNo)
    {
        var order = await _orderService.GetByIdAsync(orderNo);
        return order == null ? NotFound() : Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderDto dto)
    {
        var operatorId = User.Identity?.Name ?? string.Empty;
        var result = await _orderService.CreateAsync(dto, operatorId);
        return CreatedAtAction(nameof(GetById), new { orderNo = result.OrderNo }, result);
    }

    [HttpPut("{orderNo}")]
    public async Task<IActionResult> Update(string orderNo, [FromBody] CreateOrderDto dto)
    {
        await _orderService.UpdateAsync(orderNo, dto);
        return NoContent();
    }

    [HttpDelete("{orderNo}")]
    public async Task<IActionResult> Delete(string orderNo)
    {
        await _orderService.DeleteAsync(orderNo);
        return NoContent();
    }

    [HttpPost("{orderNo}/confirm")]
    public async Task<IActionResult> Confirm(string orderNo)
    {
        await _orderService.ConfirmAsync(orderNo);
        return NoContent();
    }

    [HttpPost("{orderNo}/invalidate")]
    public async Task<IActionResult> Invalidate(string orderNo)
    {
        await _orderService.InvalidateAsync(orderNo);
        return NoContent();
    }
}
