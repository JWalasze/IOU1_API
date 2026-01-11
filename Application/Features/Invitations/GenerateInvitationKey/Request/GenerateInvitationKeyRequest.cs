using IOU1.Application.Mediator;

namespace IOU1.Application.Features.Invitations.GenerateInvitationKey.Request;

public record GenerateInvitationKeyRequest : IRequest
{
    public long GroupId { get; set; }
}
