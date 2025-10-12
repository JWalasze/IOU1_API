namespace IOU1.Application.Features.Invitations.UseInvitationLink.Response;

public record UseInvitationLinkResponse : EndpointResponse
{
    public string HashedKey { get; init; }

    public DateTime ExpirationDate { get; init; }
}
