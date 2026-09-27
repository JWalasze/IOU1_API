namespace IOU1.Application.Persistance;

public interface IUnitOfWork
{
    Task BeginTransaction(CancellationToken cancellationToken = default);

    Task CommitTransaction(CancellationToken cancellationToken = default);

    Task RollbackTransaction();

    Task CreateSavepoint(CancellationToken cancellationToken = default);

    Task RollbackToSavepoint(CancellationToken cancellationToken = default);

    Task<int> SaveChanges(CancellationToken cancellationToken = default);
}
