using System.Linq.Expressions;

namespace BookCatalog.Domain.Abstractions;

public interface IRepository<T, TId> where T : class, IEntity<TId>
{
    Task<PagedResult<T>> GetAllAsync(PagedParameters parameters, params Expression<Func<T, object>>[] includes);
    Task<T?> GetByIdAsync(TId id, params Expression<Func<T, object>>[] includes);
    Task<PagedResult<T>> FindAsync(PagedParameters parameters, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken = default);
    Task<PagedResult<T>> GetByFilterAsync(Expression<Func<T, bool>> filter, PagedParameters parameters, params Expression<Func<T, object>>[] includes);
    Task<PagedResult<T>> GetOrderedAsync<TKey>(Expression<Func<T, TKey>> orderBy, PagedParameters parameters, bool ascending = true, params Expression<Func<T, object>>[] includes);
    Task<PagedResult<T>> GetFilteredAndOrderedAsync<TKey>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> orderBy, PagedParameters parameters, bool ascending = true, params Expression<Func<T, object>>[] includes);
}