using Application.Mediator;

namespace Application.Features.Groups.DeleteGroup.Request;

public sealed record DeleteGroupRequest : IRequest
{
    public required long GroupId { get; init; }
}
