using IOU1.Application.Features.Notifications.GetNotifications.Models.Dto;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Features.Notifications.GetNotifications.Queries;

public class GetNotificationsQuery(IOU1Context context) : IGetNotificationsQuery
{
    private readonly IOU1Context _context = context;

    public Task<List<NotificationDto>> GetAll(int userId, CancellationToken cancellationToken = default)
    {
        return _context.Notifications
            .Where(n =>
                n.UserId == userId &&
                !n.IsRead)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Payload = n.Payload,
                CreatedAt = n.CreatedAt,
                Type = n.Type,
            })
            .OrderByDescending(n => n.CreatedAt)
            .ThenByDescending(n => n.Id)
            .ToListAsync(cancellationToken);
    }
}
