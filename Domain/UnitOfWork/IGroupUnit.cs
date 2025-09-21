using Domain.RepoInterfaces;

namespace Domain.UnitOfWork;

public interface IGroupUnit
{
    IGroupRepository GroupRepository { get; }
}
