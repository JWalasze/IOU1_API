using IOU1.Application.Features.Groups.GetGroup.Models.Dto;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Features.Groups.GetGroup.Query;

public class GetGroupQuery(IOU1Context context) : IGetGroupQuery
{
    private readonly IOU1Context _context = context;

    public async Task<GetGroupDto?> GetGroup(long groupId, CancellationToken cancellationToken = default)
    {
        return await _context
            .Groups
            .Include(g => g.Members)
                .ThenInclude(m => m.User)
            .Where(g => g.Id == groupId)
            .Select(g => new GetGroupDto
            {
                GroupId = g.Id,
                OwnerName = g.Owner.FullName,
                Description = g.Description,
                Members = g.Members
                    .Select(m => new GetGroupMemberDto
                    {
                        UserId = m.Id,
                        UserName = m.User.FullName,
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
