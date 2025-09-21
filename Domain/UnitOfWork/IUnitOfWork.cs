using Domain.Base;
using Domain.Entities;
using Domain.RepoInterfaces;

namespace Domain.UnitOfWork;

public interface IUnitOfWork
{
    IRepository<T> Repository<T>() where T : Entity;

    TRepo Get<TRepo>() where TRepo : IRepository;

    Task BeginTransaction();

    Task CommitTransaction();

    Task RollbackTransaction();

    Task CreateSavepoint();

    Task RollbackToSavepoint();
}
