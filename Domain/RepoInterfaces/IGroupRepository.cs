using Domain.Entities;

namespace Domain.RepoInterfaces;

public interface IGroupRepository : IRepository<Group>
{
    Task<Group?> GetByIdAsync(long groupId);
    Task<Group?> GetGroupWithMembersAsync(long groupId);
    void Add(Group group);
}


