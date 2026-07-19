using IOU1.Domain.Entities;
using IOU1.Domain.RepoInterfaces;

namespace Domain.RepoInterfaces;

public interface IGroupRepository : IRepository<Group>
{
    Task<Group?> GetByIdAsync(int groupId, CancellationToken cancellationToken = default);
    Task<Group?> GetGroupWithMembersAsync(int groupId, CancellationToken cancellationToken = default);
}


