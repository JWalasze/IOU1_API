using IOU1.Application.Features.Notifications.MarkNotificationAsRead.Models.Request;
using IOU1.Application.Features.Notifications.MarkNotificationAsRead.Models.Response;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Notifications.MarkNotificationAsRead.Handler;

public interface IMarkNotificationAsReadHandler
{
    Task<Result<MarkNotificationAsReadResponse?>> Handle(MarkNotificationAsReadRequest request, CancellationToken cancellationToken = default);
}
