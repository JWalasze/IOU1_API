using IOU1.Application.Persistance;
using IOU1.Persistance.Context;

namespace IOU1.Infrastructure.UnitOfWork;

public class UnitOfWork(IOU1Context context) : IUnitOfWork
{
    private readonly IOU1Context _context = context;

    public async Task BeginTransaction(CancellationToken cancellationToken = default)
    {
        await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransaction(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);

        if (_context.Database.CurrentTransaction is not null)
            await _context.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransaction()
    {
        if (_context.Database.CurrentTransaction is null)
            return;

        await _context.Database.RollbackTransactionAsync();
    }

    public Task CreateSavepoint(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RollbackToSavepoint(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<int> SaveChanges(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
