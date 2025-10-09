using Domain.RepoInterfaces;
using IOU1.Domain.Entities;
using IOU1.Domain.RepoInterfaces;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Infrastructure.Repositories;

public class GroupRepository : Repository<Group>, IGroupRepository, IRepository<Group>
{
    private readonly IOU1Context _context;

    public GroupRepository(IOU1Context context) : base(context)
    {
        _context = context;
    }

    //Add BaseRepo implementation, this goes there
    public void Add(Group group)
    {
        _context.Add(group);
    }

    public void Delete(Group group)
    {
        _context.Remove(group);
    }

    public async Task<Group?> GetByIdAsync(long groupId, CancellationToken cancellationToken = default)
    {
        return await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);
    }

    public async Task<Group?> GetGroupWithMembersAsync(long groupId, CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Include(g => g.Members)
                .ThenInclude(gm => gm.User)
            .FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);
    }
}
