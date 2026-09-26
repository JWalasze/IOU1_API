using IOU1.Domain.Base;
using IOU1.Domain.ValueObjects;

namespace IOU1.Domain.Entities;

public class InvitationLink : Entity
{
    public LinkExpirationDate ExpirationDate { get; } = null!;

    public InvitationKey InvitationKey { get; } = null!;

    public Group Group { get; } = null!;

    public DateTime AddDate { get; } = DateTime.Now;

    private InvitationLink() { }

    public InvitationLink(Group group, InvitationKey key, LinkExpirationDate expirationDate) 
    { 
        Group = group;
        InvitationKey = key;
        ExpirationDate = expirationDate;
    }
}
