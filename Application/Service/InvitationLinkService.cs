using IOU1.Application.Options;
using IOU1.Domain.Entities;
using IOU1.Domain.ValueObjects;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IOU1.Application.Service;

public class InvitationLinkService(IOU1Context context, IOptionsMonitor<LinkInvitation> options) : IInvitationLinkService
{
    private readonly IOU1Context _context = context;
    private readonly LinkInvitation _linkInvitation = options.CurrentValue;

    public async Task<InvitationLink> For(long groupId, CancellationToken cancellationToken = default)
    {
        var foundGroup = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken)
            ?? throw new Exception($"Group {groupId} couldn't be found.");

        var invitationLink = new InvitationLink(foundGroup, new InvitationKey(), new LinkExpirationDate(_linkInvitation.ExpirationTime));

        _context.InvitationLinks.Add(invitationLink);
        await _context.SaveChangesAsync(cancellationToken);

        return invitationLink;
    }

    public async Task Use(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
