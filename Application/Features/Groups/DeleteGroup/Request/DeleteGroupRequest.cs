using IOU1.Application.Mediator;

namespace IOU1.Application.Features.Groups.DeleteGroup.Request;

public sealed record DeleteGroupRequest : IRequest
{
    public required long GroupId { get; init; }
}
