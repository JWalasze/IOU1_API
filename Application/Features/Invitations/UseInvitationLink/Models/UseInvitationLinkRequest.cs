using IOU1.Application.Mediator;

namespace IOU1.Application.Features.Invitations.UseInvitationLink.Models;

public record UseInvitationLinkRequest : IRequest
{
    public required int? InvitationId { get; init; }

    public required string? InvitationKey { get; init; }
}
