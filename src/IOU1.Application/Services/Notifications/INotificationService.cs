namespace IOU1.Application.Services.Notifications;

public interface INotificationService
{
    Task SendToUser(
        int userId,
        int notificationId,
        string payload,
        CancellationToken cancellationToken = default
    );
}
