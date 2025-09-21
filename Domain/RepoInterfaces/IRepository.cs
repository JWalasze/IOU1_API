using Domain.Base;

namespace Domain.RepoInterfaces;

public interface IRepository<T> : IRepository where T : Entity 
{
    
}
