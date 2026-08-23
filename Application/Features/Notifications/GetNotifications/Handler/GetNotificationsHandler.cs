using IOU1.Application.Features.Notifications.GetNotifications.Models.Dto;
using IOU1.Application.Features.Notifications.GetNotifications.Queries;
using IOU1.Domain.Models.Auth.User;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Notifications.GetNotifications.Handler;

public sealed class GetNotificationsHandler(
    IAuthUser user,
    IGetNotificationsQuery query)
    : IGetNotificationsHandler
{
    private readonly IAuthUser _user = user;
    private readonly IGetNotificationsQuery _query = query;

    public async Task<Result<ICollection<NotificationDto>>> Handle(CancellationToken cancellationToken = default)
    {
        var result = await _query.GetAll(_user.Id, cancellationToken);
        return Result<ICollection<NotificationDto>>.Success(result);
    }
}
