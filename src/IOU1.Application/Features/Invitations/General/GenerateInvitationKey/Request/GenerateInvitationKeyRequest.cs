using IOU1.Application.Mediator;

namespace IOU1.Application.Features.Invitations.General.GenerateInvitationKey.Request;

public record GenerateInvitationKeyRequest : IRequest
{
    public int GroupId { get; set; }
}
