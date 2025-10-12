using IOU1.Domain.Entities;

namespace IOU1.Application.Service;

public interface IInvitationLinkService
{
    Task<InvitationLink> For(long groupId, CancellationToken cancellationToken = default);

    Task Use(CancellationToken cancellationToken = default);
}
