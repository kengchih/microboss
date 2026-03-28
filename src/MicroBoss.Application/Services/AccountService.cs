using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using MicroBoss.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MicroBoss.Application.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<List<AccountDto>> GetAllAccountsAsync()
    {
        var users = _userManager.Users.ToList();
        var result = new List<AccountDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add(new AccountDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsEnabled = user.IsEnabled,
                LastLoginTime = user.LastLoginTime,
                RoleName = roles.FirstOrDefault()
            });
        }
        return result;
    }

    public async Task<AccountDto?> GetAccountByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return null;
        var roles = await _userManager.GetRolesAsync(user);
        return new AccountDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            IsEnabled = user.IsEnabled,
            LastLoginTime = user.LastLoginTime,
            RoleName = roles.FirstOrDefault()
        };
    }

    public async Task<AccountDto> CreateAccountAsync(CreateAccountDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.UserId,
            Email = dto.Email,
            IsEnabled = dto.IsEnabled
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        if (!string.IsNullOrEmpty(dto.RoleName))
            await _userManager.AddToRoleAsync(user, dto.RoleName);

        return new AccountDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            IsEnabled = user.IsEnabled,
            RoleName = dto.RoleName
        };
    }

    public async Task UpdateAccountAsync(string id, UpdateAccountDto dto)
    {
        var user = await _userManager.FindByIdAsync(id)
            ?? throw new InvalidOperationException("使用者不存在");

        if (dto.UserName != null) user.UserName = dto.UserName;
        if (dto.Email != null) user.Email = dto.Email;
        if (dto.IsEnabled.HasValue) user.IsEnabled = dto.IsEnabled.Value;

        await _userManager.UpdateAsync(user);

        if (dto.RoleName != null)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, dto.RoleName);
        }
    }

    public async Task DeleteAccountAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id)
            ?? throw new InvalidOperationException("使用者不存在");
        await _userManager.DeleteAsync(user);
    }

    public async Task ResetPasswordAsync(string id, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(id)
            ?? throw new InvalidOperationException("使用者不存在");
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<bool> CheckUserIdExistAsync(string userId)
    {
        var user = await _userManager.FindByNameAsync(userId);
        return user != null;
    }

    public async Task<List<RoleDto>> GetAllRolesAsync()
    {
        return _roleManager.Roles
            .Select(r => new RoleDto { Id = r.Id, Name = r.Name! })
            .ToList();
    }
}
