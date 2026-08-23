namespace IOU1.Application.Services.Notifications;

public interface INotificationService
{
    Task SendToUser(int userId, string payload, CancellationToken cancellationToken = default);
}
