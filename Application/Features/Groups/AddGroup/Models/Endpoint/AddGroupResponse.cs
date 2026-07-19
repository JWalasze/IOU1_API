namespace IOU1.Application.Features.Groups.AddGroup.Models.Endpoint;

public sealed record AddGroupResponse : EndpointResponse
{
    public int GroupId { get; init; }
}
