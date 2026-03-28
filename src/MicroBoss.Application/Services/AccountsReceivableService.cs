using MapsterMapper;
using MicroBoss.Application.Common;
using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using MicroBoss.Domain.Entities;
using MicroBoss.Domain.Enums;
using MicroBoss.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroBoss.Application.Services;

public class AccountsReceivableService : IAccountsReceivableService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public AccountsReceivableService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedResult<AccountsReceivableDto>> QueryAsync(AccountsReceivableQueryDto query)
    {
        var q = _uow.Repository<AccountsReceivable>().Query()
            .Include(ar => ar.Details)
            .Include(ar => ar.Postings)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.CustomerId))
            q = q.Where(ar => ar.CustomerId == query.CustomerId);

        if (query.Status.HasValue)
            q = q.Where(ar => ar.Status == query.Status.Value);

        if (query.Year.HasValue)
            q = q.Where(ar => ar.SettleDate.Year == query.Year.Value);

        if (query.Month.HasValue)
            q = q.Where(ar => ar.SettleDate.Month == query.Month.Value);

        var totalCount = await q.CountAsync();

        var items = await q
            .OrderByDescending(ar => ar.SettleDate)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResult<AccountsReceivableDto>
        {
            Items = _mapper.Map<List<AccountsReceivableDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<AccountsReceivableDto?> GetByIdAsync(int id)
    {
        var ar = await _uow.Repository<AccountsReceivable>().Query()
            .Include(a => a.Details)
            .Include(a => a.Postings)
            .FirstOrDefaultAsync(a => a.Id == id);

        return ar == null ? null : _mapper.Map<AccountsReceivableDto>(ar);
    }

    public async Task AddPostingAsync(int arId, CreateAccountsReceivablePostingDto dto, string operatorId)
    {
        var ar = await _uow.Repository<AccountsReceivable>().Query()
            .Include(a => a.Postings)
            .FirstOrDefaultAsync(a => a.Id == arId)
            ?? throw new InvalidOperationException("應收帳款不存在");

        var posting = new AccountsReceivablePosting
        {
            AccountsReceivableId = arId,
            PostingDate = dto.PostingDate,
            Type = (AccountsReceivablePostingType)dto.Type,
            Amount = dto.Amount,
            Account = dto.Account,
            Bank = dto.Bank,
            CheckNumber = dto.CheckNumber,
            CheckDate = dto.CheckDate,
            Note = dto.Note,
            CreatedTime = DateTime.Now,
            OperatorId = operatorId
        };

        _uow.Repository<AccountsReceivablePosting>().Add(posting);

        // Recalculate ActualAmount from all postings (including new one)
        var existingTotal = ar.Postings.Sum(p => p.Amount);
        ar.ActualAmount = existingTotal + dto.Amount;
        ar.UpdateTime = DateTime.Now;

        if (ar.ActualAmount >= ar.TotalAmount)
            ar.Status = AccountsReceivableStatus.Paid;

        _uow.Repository<AccountsReceivable>().Update(ar);
        await _uow.SaveChangesAsync();
    }

    public async Task CloseAsync(int id)
    {
        var ar = await _uow.Repository<AccountsReceivable>().Query()
            .FirstOrDefaultAsync(a => a.Id == id)
            ?? throw new InvalidOperationException("應收帳款不存在");

        ar.Status = AccountsReceivableStatus.Paid;
        ar.UpdateTime = DateTime.Now;

        _uow.Repository<AccountsReceivable>().Update(ar);
        await _uow.SaveChangesAsync();
    }
}
