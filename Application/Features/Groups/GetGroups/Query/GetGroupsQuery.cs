using IOU1.Application.Features.Groups.GetGroups.Models.Dto;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Features.Groups.GetGroups.Query;

public class GetGroupsQuery(IOU1Context context) : IGetGroupsQuery
{
    private readonly IOU1Context _context = context;

    public async Task<ICollection<GetGroupsDto>> GetGroups(long userId, CancellationToken cancellationToken = default)
    {
        return await _context
            .GroupMembers
            .Include(gm => gm.Group)
                .ThenInclude(g => g.Owner)
            .Where(gm => gm.UserId == userId)
            .Select(gm => new GetGroupsDto
            {
                GroupId = gm.Id,
                OwnerName = gm.Group.Owner.FullName,
                Description = gm.Group.Description,
            })
            .ToListAsync(cancellationToken);
    }
}
