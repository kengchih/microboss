using System.Linq.Expressions;
using MicroBoss.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroBoss.Infrastructure.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly MicroBossDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(MicroBossDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(params object[] keyValues) => await _dbSet.FindAsync(keyValues);
    public async Task<List<T>> GetAllAsync() => await _dbSet.ToListAsync();
    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();
    public IQueryable<T> Query() => _dbSet.AsQueryable();
    public void Add(T entity) => _dbSet.Add(entity);
    public void Update(T entity) => _dbSet.Update(entity);
    public void Remove(T entity) => _dbSet.Remove(entity);
}
