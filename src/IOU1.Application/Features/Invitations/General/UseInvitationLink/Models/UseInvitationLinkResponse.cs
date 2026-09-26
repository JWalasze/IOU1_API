namespace IOU1.Application.Features.Invitations.General.UseInvitationLink.Models;

public record UseInvitationLinkResponse : EndpointResponse
{
    public string HashedKey { get; init; }

    public DateTime ExpirationDate { get; init; }
}
