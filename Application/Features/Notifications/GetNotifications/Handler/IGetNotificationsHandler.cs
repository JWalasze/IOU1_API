using IOU1.Application.Features.Notifications.GetNotifications.Models.Dto;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Notifications.GetNotifications.Handler;

public interface IGetNotificationsHandler
{
    Task<Result<ICollection<NotificationDto>>> Handle(CancellationToken cancellationToken = default);
}
