namespace Application.Features.Groups.GetGroups.Response;

public sealed record GroupsResponse : EndpointResponse
{
    public required ICollection<GroupInfoResponse> GroupInfoResponse { get; init; }
}
