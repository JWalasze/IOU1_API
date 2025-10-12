using Domain.Entities;
using IOU1.Domain.Base;
using IOU1.Domain.Enums;

namespace IOU1.Domain.Entities;

public class Invitation : Entity
{
    public Group Group { get; } = null!;
    public long GroupId { get; }
    public User User { get; } = null!;
    public long UserId { get; }
    public User Sender { get; } = null!;
    public long SenderId { get; }

    public InvitationStatus InvitationStatus;

    public Invitation(long groupId, long userId, long senderId)
    {
        GroupId = groupId;
        UserId = userId;
        SenderId = senderId;
        InvitationStatus = InvitationStatus.Pending;
    }
}
