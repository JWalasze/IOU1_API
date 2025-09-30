using Domain.Entities;

namespace Domain.RepoInterfaces;

public interface IGroupRepository : IRepository<Group>
{
    Task<Group?> GetByIdAsync(long groupId, CancellationToken cancellationToken = default);
    Task<Group?> GetGroupWithMembersAsync(long groupId, CancellationToken cancellationToken = default);
    void Add(Group group);
    void Delete(Group group);
}


