using IOU1.Application.Mediator;

namespace IOU1.Application.Features.Groups.AddGroup.Models.Endpoint;

public sealed record AddGroupRequest : IRequest
{
    public string Name { get; init; } = null!;

    public string? Description { get; init; }

    public required string CurrencyKey { get; init; }

    public int OwnerId { get; init; }

    public IEnumerable<int> MemberIds { get; init; } = [];
}
