using IOU1.Application.Features.Notifications.GetNotifications.Handler;
using IOU1.Application.Features.Notifications.MarkNotificationAsRead.Handler;
using IOU1.Application.Features.Notifications.MarkNotificationAsRead.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IOU1.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class NotificationController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetNotifications(
        [FromServices] IGetNotificationsHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(cancellationToken);
        return CreateEndpointResponse(result);
    }

    [HttpPatch("{NotificationId}")]
    public async Task<IActionResult> MarkNotificationAsRead(
        [FromRoute] MarkNotificationAsReadRequest request,
        [FromServices] IMarkNotificationAsReadHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);
        return CreateEndpointResponse(result);
    }
}
