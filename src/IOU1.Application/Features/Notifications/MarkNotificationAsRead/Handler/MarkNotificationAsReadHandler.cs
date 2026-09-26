using IOU1.Application.Features.Notifications.MarkNotificationAsRead.Models.Request;
using IOU1.Application.Features.Notifications.MarkNotificationAsRead.Models.Response;
using IOU1.Domain.Models.Auth.User;
using IOU1.Domain.Models.Results;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Features.Notifications.MarkNotificationAsRead.Handler;

public sealed class MarkNotificationAsReadHandler(
    IAuthUser user,
    IOU1Context context) : IMarkNotificationAsReadHandler
{
    public async Task<Result<MarkNotificationAsReadResponse?>> Handle(MarkNotificationAsReadRequest request, CancellationToken cancellationToken = default)
    {
        var notification = await context.Notifications
            .Where(n => n.Id == request.NotificationId)
            .FirstOrDefaultAsync(cancellationToken);

        if (notification is null)
            return Result<MarkNotificationAsReadResponse?>.Failure($"A notification with id: {request.NotificationId} doesn't exist!");

        if (notification.UserId != user.Id)
            return Result<MarkNotificationAsReadResponse?>.Failure($"You can't modify someone's else notification!");

        if (notification.IsRead)
            return Result<MarkNotificationAsReadResponse?>.Failure("Notification is already marked as read!");

        notification.MarkAsRead();
        context.Notifications.Update(notification);
        await context.SaveChangesAsync(cancellationToken);

        return Result<MarkNotificationAsReadResponse?>.Success(new MarkNotificationAsReadResponse());
    }
}
