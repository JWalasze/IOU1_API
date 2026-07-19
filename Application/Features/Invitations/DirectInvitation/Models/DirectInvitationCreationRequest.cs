using IOU1.Application.Mediator;

namespace IOU1.Application.Features.Invitations.DirectInvitation.Models;

public sealed record DirectInvitationCreationRequest(
    int GroupId,
    int SenderId,
    string email
) : IRequest;
