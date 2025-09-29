using Application.Mediator;

namespace Application.Features.AddGroup.Request;

public sealed record AddGroupRequest : IRequest
{
    public IEnumerable<long> MemberIds { get; } = [];

    public string? Description { get; init; }

    public long OwnerId { get; init; }
}
