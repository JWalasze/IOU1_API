using Domain.Base;
using Domain.RepoInterfaces;
using Domain.UnitOfWork;
using Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.UnitOfWork;

public class UnitOfWork(IOU1Context context, IServiceProvider serviceProvider) : IUnitOfWork
{
    private readonly IOU1Context _context = context;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IRepository<T> Repository<T>() where T : Entity
    {
        return _serviceProvider.GetRequiredService<IRepository<T>>();
    }

    public TRepo Get<TRepo>() where TRepo : IRepository
    {
        return _serviceProvider.GetRequiredService<TRepo>();
    }

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

    public Task CreateSavepoint()
    {
        throw new NotImplementedException();
    }

    public Task RollbackToSavepoint()
    {
        throw new NotImplementedException();
    }
}
