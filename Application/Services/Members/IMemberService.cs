using IOU1.Domain.Entities;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Services.Members;

public interface IMemberService
{
    Task<Result<GroupMember?>> AddMember(long groupId, long memberId, CancellationToken cancellationToken = default);

    Task<bool> IsMemberOfGroup(long groupId, long memberId, CancellationToken cancellationToken = default);
}
