using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroBoss.Web.Controllers;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IAccountService _accountService;

    public UsersController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<ActionResult<List<AccountDto>>> GetAll()
        => Ok(await _accountService.GetAllAccountsAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDto>> GetById(string id)
    {
        var account = await _accountService.GetAccountByIdAsync(id);
        return account == null ? NotFound() : Ok(account);
    }

    [HttpPost]
    public async Task<ActionResult<AccountDto>> Create(CreateAccountDto dto)
    {
        var result = await _accountService.CreateAccountAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdateAccountDto dto)
    {
        await _accountService.UpdateAccountAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _accountService.DeleteAccountAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/reset-password")]
    public async Task<IActionResult> ResetPassword(string id, [FromBody] ResetPasswordRequest request)
    {
        await _accountService.ResetPasswordAsync(id, request.NewPassword);
        return NoContent();
    }

    [HttpGet("check-exists/{userId}")]
    public async Task<ActionResult<bool>> CheckExists(string userId)
        => Ok(await _accountService.CheckUserIdExistAsync(userId));

    [HttpGet("roles")]
    public async Task<ActionResult<List<RoleDto>>> GetRoles()
        => Ok(await _accountService.GetAllRolesAsync());
}

public record ResetPasswordRequest(string NewPassword);
