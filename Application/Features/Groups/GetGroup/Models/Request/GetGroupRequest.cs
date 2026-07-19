namespace IOU1.Application.Features.Groups.GetGroup.Models.Request;

public sealed record GetGroupRequest
{
    public required int GroupId { get; init; }
}
