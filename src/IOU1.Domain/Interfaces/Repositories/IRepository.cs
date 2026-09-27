using IOU1.Domain.Base;

namespace IOU1.Domain.Interfaces.Repositories;

//Let's keep it as some other approach when it comes to DbContext
public interface IRepository<T> where T : Entity
{
    void Add(T entity);

    void Update(T entity);

    void Delete(T entity);

    Task<int> SaveChanges(CancellationToken cancellationToken = default);

    Task<T?> GetById(int id, CancellationToken cancellationToken = default);

    Task<T?> Find(int id, CancellationToken cancellationToken = default);
}
