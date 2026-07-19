using IOU1.Domain.Base;
using IOU1.Domain.Enums;

namespace IOU1.Domain.Entities;

public class Invitation : Entity
{
    public Group Group { get; } = null!;
    public int GroupId { get; }
    public User User { get; } = null!;
    public int UserId { get; }
    public User Sender { get; } = null!;
    public int SenderId { get; }

    public InvitationStatus InvitationStatus;

    public Invitation(int groupId, int userId, int senderId)
    {
        GroupId = groupId;
        UserId = userId;
        SenderId = senderId;
        InvitationStatus = InvitationStatus.Pending;
    }
}
