using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroBoss.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ComCodesController : ControllerBase
{
    private readonly IComCodeService _service;

    public ComCodesController(IComCodeService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<ComCodeDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{groupId}")]
    public async Task<ActionResult<List<ComCodeDto>>> GetByGroup(string groupId)
        => Ok(await _service.GetByGroupAsync(groupId));

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateComCodeDto dto)
    {
        await _service.CreateAsync(dto);
        return Created();
    }

    [HttpPut("{groupId}/{codeId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(string groupId, string codeId, UpdateComCodeDto dto)
    {
        await _service.UpdateAsync(groupId, codeId, dto);
        return NoContent();
    }

    [HttpDelete("{groupId}/{codeId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string groupId, string codeId)
    {
        await _service.DeleteAsync(groupId, codeId);
        return NoContent();
    }
}
