using Application.Mediator;

namespace Application.Features.Groups.AddGroup.Request;

public sealed record AddGroupRequest : IRequest
{
    public IEnumerable<long> MemberIds { get; } = [];

    public string? Description { get; init; }

    public long OwnerId { get; init; }
}
