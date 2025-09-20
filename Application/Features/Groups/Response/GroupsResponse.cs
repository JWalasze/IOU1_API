using Application.Mediator;

namespace Application.Features.Groups.Response;

public sealed record GroupsResponse : IResponse
{
    public required ICollection<GroupInfoResponse> GroupInfoResponse { get; init; }
}
