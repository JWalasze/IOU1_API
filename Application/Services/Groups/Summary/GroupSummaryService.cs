using IOU1.Domain.Models.Groups.Summary;

namespace IOU1.Application.Services.Groups.Summary;

public class GroupSummaryService : IGroupSummaryService
{
    public Task<GroupSummary> GetGroupSummary(int groupId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
