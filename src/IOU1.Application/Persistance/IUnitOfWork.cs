namespace IOU1.Application.Persistance;

public interface IUnitOfWork
{
    Task BeginTransaction();

    Task CommitTransaction();

    Task RollbackTransaction();

    Task CreateSavepoint();

    Task RollbackToSavepoint();

    Task<int> SaveChanges(CancellationToken cancellationToken = default);
}
