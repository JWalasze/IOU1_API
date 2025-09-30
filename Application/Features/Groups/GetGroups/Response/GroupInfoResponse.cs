namespace Application.Features.Groups.GetGroups.Response;

public sealed record GroupInfoResponse(
    long Id,
    string Description,
    string OwnerName)
{
}
