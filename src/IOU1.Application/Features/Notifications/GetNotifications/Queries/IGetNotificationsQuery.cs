using IOU1.Application.Features.Notifications.GetNotifications.Models.Dto;

namespace IOU1.Application.Features.Notifications.GetNotifications.Queries;

public interface IGetNotificationsQuery
{
    Task<List<NotificationDto>> GetAll(int userId, CancellationToken cancellationToken = default);
}
