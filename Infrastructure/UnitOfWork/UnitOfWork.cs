using Domain.UnitOfWork;

namespace Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    //Method where you get any repo you need

    public Task BeginTransaction()
    {
        throw new NotImplementedException();
    }

    public Task CommitTransaction()
    {
        throw new NotImplementedException();
    }

    public Task RollbackTransaction()
    {
        throw new NotImplementedException();
    }
}
