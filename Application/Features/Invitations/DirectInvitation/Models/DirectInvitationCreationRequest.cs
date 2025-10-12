using Application.Mediator;

namespace IOU1.Application.Features.Invitations.DirectInvitation.Models;

public sealed record DirectInvitationCreationRequest(
    long GroupId,
    long SenderId,
    string email
) : IRequest;
