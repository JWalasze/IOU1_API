using Domain.Entities;
using Domain.RepoInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GroupRepository : IGroupRepository, IRepository<Group>
{
    private readonly IOU1Context _context;

    public GroupRepository(IOU1Context context)
    {
        _context = context;
    }

    public void Add(Group group)
    {
        _context.Add(group);
    }

    public async Task<Group?> GetByIdAsync(long groupId)
    {
        return await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
    }

    public async Task<Group?> GetGroupWithMembersAsync(long groupId)
    {
        return await _context.Groups
            .Include(g => g.Members)
                .ThenInclude(gm => gm.User)
            .FirstOrDefaultAsync(g => g.Id == groupId);
    }
}
