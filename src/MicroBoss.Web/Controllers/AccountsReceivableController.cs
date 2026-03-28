using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroBoss.Web.Controllers;

[ApiController]
[Route("api/accounts-receivable")]
[Authorize]
public class AccountsReceivableController : ControllerBase
{
    private readonly IAccountsReceivableService _arService;

    public AccountsReceivableController(IAccountsReceivableService arService)
    {
        _arService = arService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<AccountsReceivableDto>>> Query([FromQuery] AccountsReceivableQueryDto query)
        => Ok(await _arService.QueryAsync(query));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AccountsReceivableDto>> GetById(int id)
    {
        var ar = await _arService.GetByIdAsync(id);
        return ar == null ? NotFound() : Ok(ar);
    }

    [HttpPost("{id:int}/postings")]
    public async Task<IActionResult> AddPosting(int id, [FromBody] CreateAccountsReceivablePostingDto dto)
    {
        var operatorId = User.Identity?.Name ?? string.Empty;
        await _arService.AddPostingAsync(id, dto, operatorId);
        return NoContent();
    }

    [HttpPost("{id:int}/close")]
    public async Task<IActionResult> Close(int id)
    {
        await _arService.CloseAsync(id);
        return NoContent();
    }
}
