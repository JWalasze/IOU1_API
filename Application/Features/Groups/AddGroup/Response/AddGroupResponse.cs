using Application.Features.Groups.GetGroups.Response;

namespace Application.Features.Groups.AddGroup.Response;

public sealed record AddGroupResponse : EndpointResponse
{
    public long GroupId { get; init; }
}
