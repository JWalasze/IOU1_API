namespace IOU1.Application.Features.Invitations.Direct.AddDirectInvitation.Models.Request;

public sealed record AddDirectInvitationRequest(
    int GroupId,
    string Email);
