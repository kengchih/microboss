using System.Collections.Concurrent;
using MicroBoss.Domain.Interfaces;

namespace MicroBoss.Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MicroBossDbContext _context;
    private readonly ConcurrentDictionary<Type, object> _repositories = new();

    public UnitOfWork(MicroBossDbContext context) { _context = context; }

    public IRepository<T> Repository<T>() where T : class
        => (IRepository<T>)_repositories.GetOrAdd(typeof(T), _ => new Repository<T>(_context));

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public void Dispose() => _context.Dispose();
}
