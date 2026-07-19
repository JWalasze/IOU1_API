using IOU1.Application.Features.Invitations.DirectInvitation.Models;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Invitations.DirectInvitation;

public interface IDirectInvitationCreationHandler
{
    Task<Result<DirectInvitationCreationResponse?>> Handle(DirectInvitationCreationRequest request, CancellationToken cancellationToken = default);
}
