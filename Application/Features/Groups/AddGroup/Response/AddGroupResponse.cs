using IOU1.Application;

namespace Application.Features.Groups.AddGroup.Response;

public sealed record AddGroupResponse : EndpointResponse
{
    public long GroupId { get; init; }
}
