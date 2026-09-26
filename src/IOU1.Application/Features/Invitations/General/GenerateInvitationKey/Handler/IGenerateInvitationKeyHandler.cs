using IOU1.Application.Features.Invitations.General.GenerateInvitationKey.Request;
using IOU1.Application.Features.Invitations.General.UseInvitationLink.Models;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Invitations.General.GenerateInvitationKey.Handler;

public interface IGenerateInvitationKeyHandler
{
    Task<Result<UseInvitationLinkResponse?>> Handle(GenerateInvitationKeyRequest request, CancellationToken cancellationToken = default);
}
