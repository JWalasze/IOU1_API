using Application.Mediator;

namespace Application.Features.AddGroup.Request;

public record AddGroupRequest : IRequest
{
    public IEnumerable<long> MemberIds { get; } = [];
}
