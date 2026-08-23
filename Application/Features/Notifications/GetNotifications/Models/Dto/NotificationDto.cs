using IOU1.Domain.Enums;

namespace IOU1.Application.Features.Notifications.GetNotifications.Models.Dto;

public sealed record NotificationDto
{
    public required int Id { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required NotificationType Type { get; init; }
    public required string Payload { get; init; }
}
