using IOU1.Domain.Models.Groups.Summary;

namespace IOU1.Application.Services.Groups.Summary;

public interface IGroupSummaryService
{
    Task<GroupSummary> GetGroupSummary(long groupId, CancellationToken cancellationToken = default);
}
