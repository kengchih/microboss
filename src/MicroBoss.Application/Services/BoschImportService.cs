using MicroBoss.Application.DTOs;
using MicroBoss.Application.Interfaces;
using MicroBoss.Domain.Entities;
using MicroBoss.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroBoss.Application.Services;

public class BoschImportService : IBoschImportService
{
    private readonly IUnitOfWork _uow;

    public BoschImportService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task ImportAsync(List<SpSourceDataDto> items)
    {
        var repo = _uow.Repository<SpSourceData>();

        foreach (var item in items)
        {
            if (string.IsNullOrWhiteSpace(item.PN))
                continue;

            var existing = await repo.Query()
                .FirstOrDefaultAsync(s => s.PN == item.PN);

            if (existing != null)
            {
                existing.PG = item.PG;
                existing.Class = item.Class;
                existing.TWD = item.TWD;
                existing.ENDesc = item.ENDesc;
                existing.CNDesc = item.CNDesc;
                existing.Successor = item.Successor;
                existing.LastUpdateTime = DateTime.Now;
                repo.Update(existing);
            }
            else
            {
                var entity = new SpSourceData
                {
                    PN = item.PN,
                    PG = item.PG,
                    Class = item.Class,
                    TWD = item.TWD,
                    ENDesc = item.ENDesc,
                    CNDesc = item.CNDesc,
                    Successor = item.Successor,
                    LastUpdateTime = DateTime.Now
                };
                repo.Add(entity);
            }
        }

        await _uow.SaveChangesAsync();
    }

    public async Task ResetAsync()
    {
        var repo = _uow.Repository<SpSourceData>();
        var all = await repo.Query().ToListAsync();
        foreach (var item in all)
            repo.Remove(item);

        await _uow.SaveChangesAsync();
    }
}
