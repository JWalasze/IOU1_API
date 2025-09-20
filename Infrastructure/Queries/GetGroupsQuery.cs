using Application.Features.Groups;
using Application.Features.Groups.Dtos;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries;

public class GetGroupsQuery(IOU1Context context) : IGetGroupsQuery
{
    private readonly IOU1Context _context = context;

    public async Task<ICollection<GetGroupsDto>> GetGroups(long userId)
    {
        return await _context
            .GroupMembers
            .Include(gm => gm.Group)
                .ThenInclude(g => g.Owner)
            .Where(gm => gm.MemberId == userId)
            .Select(gm => new GetGroupsDto
            {
                GroupId = gm.Id,
                OwnerName = gm.Group.Owner.FullName,
                Description = gm.Group.Description,
            })
            .ToListAsync();
    }
}
