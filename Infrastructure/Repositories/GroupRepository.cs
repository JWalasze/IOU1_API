using Domain.RepoInterfaces;
using IOU1.Domain.Entities;
using IOU1.Domain.RepoInterfaces;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Infrastructure.Repositories;

public class GroupRepository(IOU1Context context)
    : Repository<Group>(context), IGroupRepository, IRepository<Group>
{
    private readonly IOU1Context _context = context;

    public Task<Group?> GetGroupWithMembers(long groupId, CancellationToken cancellationToken = default)
    {
        return _context.Groups
            .Include(g => g.Members)
            .Where(g => g.Id == groupId)
            .SingleOrDefaultAsync(cancellationToken);
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
