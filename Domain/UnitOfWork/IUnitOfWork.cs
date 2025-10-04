using Domain.Entities;
using IOU1.Domain.Base;
using IOU1.Domain.RepoInterfaces;

namespace IOU1.Domain.UnitOfWork;

public interface IUnitOfWork
{
    IRepository<T> Repository<T>() where T : Entity;

    TRepo Get<TRepo>() where TRepo : IRepository;

    Task BeginTransaction();

    Task CommitTransaction();

    Task RollbackTransaction();

    Task CreateSavepoint();

    Task RollbackToSavepoint();

    Task<int> SaveChanges(CancellationToken cancellationToken = default);
}
