namespace IOU1.Application.Features.Groups.GetGroupSummary.Models.Request;

public sealed record GetGroupSummaryRequest
{
    public required long GroupId { get; init; }
}
