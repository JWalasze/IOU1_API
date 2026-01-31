namespace IOU1.Application.Features.Groups.GetGroup.Models.Request;

public sealed record GetGroupRequest
{
    public required long GroupId { get; init; }
}
