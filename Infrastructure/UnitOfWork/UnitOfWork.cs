using IOU1.Domain.UnitOfWork;
using IOU1.Persistance.Context;

namespace IOU1.Infrastructure.UnitOfWork;

public class UnitOfWork(IOU1Context context) : IUnitOfWork
{
    private readonly IOU1Context _context = context;

    public async Task BeginTransaction()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransaction()
    {
        await _context.SaveChangesAsync();
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransaction()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    public Task CreateSavepoint()
    {
        throw new NotImplementedException();
    }

    public Task RollbackToSavepoint()
    {
        throw new NotImplementedException();
    }

    public async Task<int> SaveChanges(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
