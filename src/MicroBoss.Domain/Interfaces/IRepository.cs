using System.Linq.Expressions;

namespace MicroBoss.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(params object[] keyValues);
    Task<List<T>> GetAllAsync();
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
    IQueryable<T> Query();
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
}
