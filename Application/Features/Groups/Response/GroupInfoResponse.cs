namespace Application.Features.Groups.Response;

public sealed record GroupInfoResponse(
    long Id,
    string Description,
    string OwnerName)
{
}
