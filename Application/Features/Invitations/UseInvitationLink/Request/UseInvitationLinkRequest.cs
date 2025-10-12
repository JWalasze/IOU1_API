using Application.Mediator;

namespace IOU1.Application.Features.Invitations.UseInvitationLink.Request;

public record UseInvitationLinkRequest : IRequest
{
    public required long? InvitationId { get; init; }

    public required string? InvitationKey { get; init; }
}
