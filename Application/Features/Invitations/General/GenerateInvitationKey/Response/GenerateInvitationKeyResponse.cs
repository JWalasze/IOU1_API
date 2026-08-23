namespace IOU1.Application.Features.Invitations.General.GenerateInvitationKey.Response;

public record GenerateInvitationKeyResponse : EndpointResponse
{
    public string HashedKey { get; init; }

    public DateTime ExpirationDate { get; init; }
}
