using MapsterMapper;
using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using MicroBoss.Domain.Entities;
using MicroBoss.Domain.Interfaces;

namespace MicroBoss.Application.Services;

public class ComCodeService : IComCodeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ComCodeService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<ComCodeDto>> GetByGroupAsync(string groupId)
    {
        var repo = _unitOfWork.Repository<ComCode>();
        var items = await repo.FindAsync(c => c.GroupId == groupId && (c.IsEnabled == null || c.IsEnabled == true));
        return items.OrderBy(c => c.SortIndex).Select(c => _mapper.Map<ComCodeDto>(c)).ToList();
    }

    public async Task<List<ComCodeDto>> GetAllAsync()
    {
        var repo = _unitOfWork.Repository<ComCode>();
        var items = await repo.GetAllAsync();
        return items.OrderBy(c => c.GroupId).ThenBy(c => c.SortIndex).Select(c => _mapper.Map<ComCodeDto>(c)).ToList();
    }

    public async Task CreateAsync(CreateComCodeDto dto)
    {
        var repo = _unitOfWork.Repository<ComCode>();
        var entity = new ComCode
        {
            GroupId = dto.GroupId,
            CodeId = dto.CodeId,
            CodeName = dto.CodeName,
            SortIndex = dto.SortIndex,
            IsEnabled = true,
            LastUpdateTime = DateTime.Now
        };
        repo.Add(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(string groupId, string codeId, UpdateComCodeDto dto)
    {
        var repo = _unitOfWork.Repository<ComCode>();
        var entity = await repo.GetByIdAsync(groupId, codeId)
            ?? throw new InvalidOperationException("代碼不存在");
        entity.CodeName = dto.CodeName;
        entity.SortIndex = dto.SortIndex;
        entity.LastUpdateTime = DateTime.Now;
        repo.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(string groupId, string codeId)
    {
        var repo = _unitOfWork.Repository<ComCode>();
        var entity = await repo.GetByIdAsync(groupId, codeId)
            ?? throw new InvalidOperationException("代碼不存在");
        repo.Remove(entity);
        await _unitOfWork.SaveChangesAsync();
    }
}
