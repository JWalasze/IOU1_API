using IOU1.Domain.Base;
using IOU1.Domain.Enums;
using IOU1.Domain.Exceptions;
using IOU1.Domain.Utils;

namespace IOU1.Domain.Entities.Notifications;

public class Notification : Entity
{
    public DateTime CreatedAt { get; private set; }
    public NotificationType Type { get; private set; }
    public string Payload { get; private set; } = null!;
    public bool IsRead { get; private set; }

    public int UserId { get; private set; }
    public User User { get; private set; } = null!;

    #region Ctor
    private Notification() { }

    private Notification(DateTime createdAt, string payload, int userId, NotificationType type)
    {
        GuardIntId.ForPresence<CreatingNotificationException>(userId, "Provided user ID is invalid!");
        UserId = userId;
        Type = type;

        CreatedAt = createdAt;
        Payload = payload;
        IsRead = false;
    }

    private Notification(DateTime createdAt, string payload, User user, NotificationType type)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(User));
        User = user;
        UserId = user.Id;
        Type = type;

        CreatedAt = createdAt;
        Payload = payload;
        IsRead = false;
    }
    #endregion

    #region Factories
    public static Notification Create(
        DateTime createdAt,
        string payload,
        int userId,
        NotificationType type)
        => new(createdAt, payload, userId, type);

    public static Notification Create(
        DateTime createdAt,
        string payload,
        User user,
        NotificationType type)
        => new(createdAt, payload, user, type);
    #endregion
}
