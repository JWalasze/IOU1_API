namespace IOU1.Application.Features.Invitations.Direct.AddDirectInvitation.Models.Response;

public sealed record AddDirectInvitationResponse(
    int InvitationId,
    int NotificationId) : EndpointResponse;
