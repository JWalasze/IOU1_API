using IOU1.Application.Features.Invitations.General.UseInvitationLink.Models;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Invitations.General.UseInvitationLink;

public interface IUseInvitationLinkHandler
{
    Task<Result<UseInvitationLinkResponse?>> Handle(UseInvitationLinkRequest request, CancellationToken cancellationToken = default);
}
