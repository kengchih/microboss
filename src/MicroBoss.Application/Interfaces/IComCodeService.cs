using MicroBoss.Application.DTOs;

namespace MicroBoss.Application.Interfaces;

public interface IComCodeService
{
    Task<List<ComCodeDto>> GetByGroupAsync(string groupId);
    Task<List<ComCodeDto>> GetAllAsync();
    Task CreateAsync(CreateComCodeDto dto);
    Task UpdateAsync(string groupId, string codeId, UpdateComCodeDto dto);
    Task DeleteAsync(string groupId, string codeId);
}
