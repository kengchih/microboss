using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;

namespace MicroBoss.Application.Interfaces;

public interface IAccountsReceivableService
{
    Task<PagedResult<AccountsReceivableDto>> QueryAsync(AccountsReceivableQueryDto query);
    Task<AccountsReceivableDto?> GetByIdAsync(int id);
    Task AddPostingAsync(int arId, CreateAccountsReceivablePostingDto dto, string operatorId);
    Task CloseAsync(int id);
}
