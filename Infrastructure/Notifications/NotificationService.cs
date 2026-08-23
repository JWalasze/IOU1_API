using IOU1.Application.Services.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace IOU1.Infrastructure.Notifications;

public sealed class NotificationService(
    IHubContext<NotificationHub> hubContext) : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;

    public Task SendToUser(int userId, int notificationId, string payload, CancellationToken cancellationToken = default)
    {
        return _hubContext.Clients
            .Group($"group_{userId}")
            .SendAsync("ReceiveNotification",
                payload,
                cancellationToken);
    }
}
