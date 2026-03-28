using MicroBoss.Application.DTOs;

namespace MicroBoss.Application.Interfaces;

public interface IAccountService
{
    Task<List<AccountDto>> GetAllAccountsAsync();
    Task<AccountDto?> GetAccountByIdAsync(string id);
    Task<AccountDto> CreateAccountAsync(CreateAccountDto dto);
    Task UpdateAccountAsync(string id, UpdateAccountDto dto);
    Task DeleteAccountAsync(string id);
    Task ResetPasswordAsync(string id, string newPassword);
    Task<bool> CheckUserIdExistAsync(string userId);
    Task<List<RoleDto>> GetAllRolesAsync();
}
