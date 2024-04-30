using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CleanArchitecture.Infrastructure.Repositories;
public class RepositoryBase<T> : IAsyncRepository<T> where T : BaseDomainModel
{
    protected readonly StreamerDbContext _context;

    public RepositoryBase(StreamerDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }
    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }
    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate,
                                                    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                    string includeString = null,
                                                    bool disableTracking = true)
    {
        IQueryable<T> query = _context.Set<T>();
        if (disableTracking)
        {
            query = query.AsNoTracking();
        }
        if (!string.IsNullOrEmpty(includeString))
        {
            query = query.Include(includeString);
        }
        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        return orderBy is not null ? await orderBy(query).ToListAsync() : await query.ToListAsync();
    }
    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate,
                                            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                            List<Expression<Func<T, object>>> includes = null,
                                            bool disableTracking = true)
    {
        IQueryable<T> query = _context.Set<T>();
        if (disableTracking)
        {
            query = query.AsNoTracking();
        }
        if (includes is not null)
        {
            query = includes.Aggregate(query, (cur, include) => cur.Include(include));
        }
        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        return orderBy is not null ? await orderBy(query).ToListAsync() : await query.ToListAsync();
    }
    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
    public async Task<T> AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    public async Task<T> UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }
    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}
