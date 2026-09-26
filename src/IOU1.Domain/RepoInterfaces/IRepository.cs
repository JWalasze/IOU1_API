using IOU1.Domain.Base;
using System.Linq.Expressions;

namespace IOU1.Domain.RepoInterfaces;

public delegate IQueryable<T> QueryComposer<T>(IQueryable<T> source);

public interface IRepository<T> : IRepository where T : Entity
{
    void Add(T entity);

    void Update(T entity);

    void Delete(T entity);

    Task<int> SaveChanges(CancellationToken cancellationToken = default);

    Task<T?> GetById(int id, CancellationToken cancellationToken = default);

    Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> filter, QueryComposer<T>? toBeIncluded = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool shouldBeTracked = true, CancellationToken cancellation = default);

    Task<T?> GetSingleOrDefault(Expression<Func<T, bool>> filter, QueryComposer<T>? toBeIncluded = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool shouldBeTracked = true, CancellationToken cancellation = default);

    Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter, QueryComposer<T>? toBeIncluded = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool shouldBeTracked = true, CancellationToken cancellation = default);

    Task<T?> Find(int id, CancellationToken cancellationToken = default);
}
