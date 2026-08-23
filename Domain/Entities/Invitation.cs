using IOU1.Domain.Base;
using IOU1.Domain.Enums;
using IOU1.Domain.Exceptions;
using IOU1.Domain.Utils;

namespace IOU1.Domain.Entities;

public class Invitation : Entity
{
    public int GroupId { get; }
    public Group Group { get; } = null!;

    public int UserId { get; }
    public User User { get; } = null!;

    public int SenderId { get; }
    public User Sender { get; } = null!;

    public InvitationStatus InvitationStatus;

    #region Ctor
    private Invitation() { }

    private Invitation(int groupId, int userId, int senderId)
    {
        GuardIntId.ForPresence<CreatingInvitationException>(groupId,
            "GroupId for an invitation cannot be empty.");
        GroupId = groupId;

        GuardIntId.ForPresence<CreatingInvitationException>(userId,
            "GroupId for a expense category cannot be empty.");
        UserId = userId;

        GuardIntId.ForPresence<CreatingInvitationException>(senderId,
            "GroupId for a expense category cannot be empty.");
        SenderId = senderId;

        InvitationStatus = InvitationStatus.Pending;
    }

    private Invitation(
        Group group,
        User userToBeAdded,
        User sender)
    {
        ArgumentNullException.ThrowIfNull(group);
        ArgumentNullException.ThrowIfNull(userToBeAdded);
        ArgumentNullException.ThrowIfNull(sender);

        Group = group;
        GroupId = group.Id;

        User = userToBeAdded;
        UserId = userToBeAdded.Id;

        Sender = sender;
        SenderId = sender.Id;

        InvitationStatus = InvitationStatus.Pending;
    }
    #endregion

    #region Factories
    public static Invitation Create(
        int groupId, int userId, int senderId)
        => new(groupId, userId, senderId);

    public static Invitation Create(
        Group group,
        User userToBeAdded,
        User sender)
        => new(group, userToBeAdded, sender);
    #endregion

    #region Public Methods
    public void MarkAsAccepted()
    {
        InvitationStatus = InvitationStatus switch
        {
            InvitationStatus.Accepted => throw new InvitationStatusException("Invitation is already accepted!"),
            InvitationStatus.Canceled => throw new InvitationStatusException("Invitation is canceled!"),
            InvitationStatus.Pending => InvitationStatus.Accepted,
            _ => throw new InvitationStatusException("Invitation has invalid status!")
        };
    }
    #endregion
}
