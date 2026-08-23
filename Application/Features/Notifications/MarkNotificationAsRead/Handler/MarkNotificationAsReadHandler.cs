using IOU1.Application.Features.Auth;
using IOU1.Application.Features.Notifications.MarkNotificationAsRead.Models.Request;
using IOU1.Application.Features.Notifications.MarkNotificationAsRead.Models.Response;
using IOU1.Domain.Models.Results;
using IOU1.Persistance.Context;

namespace IOU1.Application.Features.Notifications.MarkNotificationAsRead.Handler;

public sealed class MarkNotificationAsReadHandler(
    IAuthService authService,
    IOU1Context context) : IMarkNotificationAsReadHandler
{
    private readonly IAuthService _authService = authService;
    private readonly IOU1Context _context = context;

    public Task<Result<MarkNotificationAsReadResponse?>> Handle(MarkNotificationAsReadRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
