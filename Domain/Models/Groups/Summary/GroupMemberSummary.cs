namespace IOU1.Domain.Models.Groups.Summary;

public record GroupMemberSummary
{
    public int UserId { get; init; }
    public int MemberId { get; init; }
    public string MemberName { get; init; } = null!;

}
