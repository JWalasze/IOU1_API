using IOU1.Domain.Models;

namespace IOU1.Application.Services.Members;

public interface IMemberService
{
    Task<Result> AddMember(long groupId, long memberId, CancellationToken cancellationToken = default);
}
