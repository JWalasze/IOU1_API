using IOU1.Application.Features.Invitations.GenerateInvitationKey.Request;
using IOU1.Application.Features.Invitations.UseInvitationLink.Models;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Invitations.GenerateInvitationKey.Handler;

public interface IGenerateInvitationKeyHandler
{
    Task<Result<UseInvitationLinkResponse?>> Handle(GenerateInvitationKeyRequest request, CancellationToken cancellationToken = default);
}
