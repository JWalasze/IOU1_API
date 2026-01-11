using IOU1.Application.Mediator;

namespace IOU1.Application.Features.Groups.AddGroup.Models.Endpoint;

public sealed record AddGroupRequest : IRequest
{
    public string Name { get; init; } = null!;

    public string? Description { get; init; }

    public long OwnerId { get; init; }

    public IEnumerable<long> MemberIds { get; } = [];
}
