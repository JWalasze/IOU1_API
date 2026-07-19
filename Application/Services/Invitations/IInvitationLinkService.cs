using IOU1.Domain.Entities;
using IOU1.Domain.ValueObjects;

namespace IOU1.Application.Services.Invitations;

public interface IInvitationLinkService
{
    Task<InvitationLink> For(int groupId, CancellationToken cancellationToken = default);

    Task UseInvitationKey(InvitationKey invitationKey, CancellationToken cancellationToken = default);

    Task UseDirectInvitation(int invitationId, CancellationToken cancellationToken = default);
}
