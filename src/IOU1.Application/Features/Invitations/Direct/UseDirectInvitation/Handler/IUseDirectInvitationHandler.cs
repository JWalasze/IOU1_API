using IOU1.Application.Features.Invitations.Direct.UseDirectInvitation.Models.Request;
using IOU1.Application.Features.Invitations.Direct.UseDirectInvitation.Models.Response;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Invitations.Direct.UseDirectInvitation.Handler;

public interface IUseDirectInvitationHandler
{
    Task<Result<UseDirectInvitationResponse>> Handle(UseDirectInvitationRequest request, CancellationToken cancellationToken = default);
}
