using IOU1.Domain.Entities;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Services.Members;

public interface IMemberService
{
    Task<Result<GroupMember?>> AddMember(int groupId, int memberId, CancellationToken cancellationToken = default);

    Task<bool> IsMemberOfGroup(int groupId, int memberId, CancellationToken cancellationToken = default);
}
