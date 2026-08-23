using IOU1.Application.Features.Invitations.Direct.AddDirectInvitation.Models.Request;
using IOU1.Application.Features.Invitations.Direct.AddDirectInvitation.Models.Response;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Invitations.Direct.AddDirectInvitation.Handler;

public interface IAddDirectInvitationHandler
{
    Task<Result<AddDirectInvitationResponse?>> Handle(AddDirectInvitationRequest request, CancellationToken cancellationToken = default);
}
