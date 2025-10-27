using IOU1.Domain.Entities;
using IOU1.Domain.ValueObjects;

namespace IOU1.Application.Service;

public interface IInvitationLinkService
{
    Task<InvitationLink> For(long groupId, CancellationToken cancellationToken = default);

    Task UseInvitationKey(InvitationKey invitationKey, CancellationToken cancellationToken = default);

    Task UseDirectInvitation(long invitationId, CancellationToken cancellationToken = default);
}
