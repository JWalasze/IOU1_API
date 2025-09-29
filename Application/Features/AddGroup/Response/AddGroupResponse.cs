using Application.Features.Groups.Response;

namespace Application.Features.AddGroup.Response;

public sealed record AddGroupResponse : EndpointResponse
{
    public long GroupId { get; init; }
}
