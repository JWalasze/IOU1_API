namespace IOU1.Domain.Models.Groups.Summary;

public record GroupSummary
{
    public int GroupId { get; init; }
    public IEnumerable<GroupMemberSummary> MembersSummary { get; init; } = [];
}
