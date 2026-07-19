using IOU1.Domain.Base;
using IOU1.Domain.RepoInterfaces;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IOU1.Infrastructure.Repositories;

public class Repository<T>(IOU1Context context) : IRepository<T> where T : Entity
{
    private readonly IOU1Context _context = context;

    public void Add(T entity)
    {
        _context.Add(entity);
    }

    public void Delete(T entity)
    {
        _context.Remove(entity);
    }

    public void Update(T entity)
    {
        _context.Update(entity);
    }

    public async Task<T?> Find(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().FindAsync([id], cancellationToken);
    }

    public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter, QueryComposer<T>? toBeIncluded, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, bool shouldBeTracked = true, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<T>().Where(filter);
        
        if (toBeIncluded is not null)
        {
            toBeIncluded(query);
        }

        if (orderBy is not null)
        {
            orderBy(query);
        }

        if (!shouldBeTracked)
        {
            query.AsNoTracking();
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<T?> GetById(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> filter, QueryComposer<T>? toBeIncluded = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool shouldBeTracked = true, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<T>().Where(filter);

        if (toBeIncluded is not null)
        {
            toBeIncluded(query);
        }

        if (orderBy is not null)
        {
            orderBy(query);
        }

        if (!shouldBeTracked)
        {
            query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<T?> GetSingleOrDefault(Expression<Func<T, bool>> filter, QueryComposer<T>? toBeIncluded = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool shouldBeTracked = true, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<T>().Where(filter);

        if (toBeIncluded is not null)
        {
            toBeIncluded(query);
        }

        if (orderBy is not null)
        {
            orderBy(query);
        }

        if (!shouldBeTracked)
        {
            query.AsNoTracking();
        }

        return await query.SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<int> SaveChanges(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
