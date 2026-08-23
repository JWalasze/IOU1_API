namespace IOU1.Application.Features.Notifications.MarkNotificationAsRead.Models.Request;

public sealed record MarkNotificationAsReadRequest
{
    public required int NotificationId { get; init; }
}
