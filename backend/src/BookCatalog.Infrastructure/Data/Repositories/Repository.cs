using BookCatalog.Domain.Abstractions;
using BookCatalog.Infrastructure.Data.Extentions;
using BookCatalog.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookCatalog.Infrastructure.Data.Repositories;

public class Repository<T, TId>(ApplicationDbContext context) : IRepository<T, TId> where T : class, IEntity<TId>
{
    protected readonly ApplicationDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<PagedResult<T>> GetAllAsync(PagedParameters parameters, params Expression<Func<T, object>>[] includes)
    {
        var query = ApplyIncludes(_dbSet, includes);
        query = query.OrderByDescending(x => x.CreatedAt);
        return await ExecutePaginatedQueryAsync(query, parameters);
    }

    public async Task<T?> GetByIdAsync(TId id, params Expression<Func<T, object>>[] includes)
    {
        if (includes?.Length == 0)
        {
            return await _dbSet.FindAsync(new object[] { id });
        }

        var query = ApplyIncludes(_dbSet, includes);
        return await query.FirstOrDefaultAsync(x => x.Id!.Equals(id));
    }

    public async Task<PagedResult<T>> FindAsync(PagedParameters parameters, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        var query = ApplyIncludes(_dbSet.Where(predicate), includes);
        return await ExecutePaginatedQueryAsync(query, parameters);
    }

    public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        var query = ApplyIncludes(_dbSet, includes);
        return await query.SingleOrDefaultAsync(predicate);
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _dbSet.Update(entity);
        var rowsAffected = await _context.SaveChangesAsync(cancellationToken);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        if (entity == null)
            throw new NotFoundException($"Entidade com ID {id} não encontrada.");

        _dbSet.Remove(entity);
        var rowsAffected = await _context.SaveChangesAsync(cancellationToken);
        return rowsAffected > 0;
    }

    public async Task<PagedResult<T>> GetByFilterAsync(
        Expression<Func<T, bool>> filter,
        PagedParameters parameters,
        params Expression<Func<T, object>>[] includes)
    {
        var query = ApplyIncludes(_dbSet.Where(filter), includes);
        return await ExecutePaginatedQueryAsync(query, parameters);
    }

    public async Task<PagedResult<T>> GetOrderedAsync<TKey>(
        Expression<Func<T, TKey>> orderBy,
        PagedParameters parameters,
        bool ascending = true,
        params Expression<Func<T, object>>[] includes)
    {
        var query = ApplyIncludes(_dbSet, includes);

        query = ascending
            ? query.OrderBy(orderBy)
            : query.OrderByDescending(orderBy);

        return await ExecutePaginatedQueryAsync(query, parameters);
    }

    public async Task<PagedResult<T>> GetFilteredAndOrderedAsync<TKey>(
        Expression<Func<T, bool>> filter,
        Expression<Func<T, TKey>> orderBy,
        PagedParameters parameters,
        bool ascending = true,
        params Expression<Func<T, object>>[] includes)
    {
        var query = ApplyIncludes(_dbSet.Where(filter), includes);

        query = ascending
            ? query.OrderBy(orderBy)
            : query.OrderByDescending(orderBy);

        return await ExecutePaginatedQueryAsync(query, parameters);
    }

    private static IQueryable<T> ApplyIncludes(IQueryable<T> query, Expression<Func<T, object>>[]? includes)
    {
        if (includes?.Length > 0)
        {
            query = query.AsSplitQuery();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return query;
    }

    private async Task<PagedResult<T>> ExecutePaginatedQueryAsync(IQueryable<T> query, PagedParameters parameters)
    {
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return items.ToPagedResult(parameters, totalCount);
    }
}