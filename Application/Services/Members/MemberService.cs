using IOU1.Domain.Entities;
using IOU1.Domain.Models;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Services.Members;

public class MemberService(IOU1Context context) : IMemberService
{
    private readonly IOU1Context _context = context;

    public async Task<Result> AddMember(long groupId, long memberId, CancellationToken cancellationToken = default)
    {
        var groupp = await _context.Groups
            .Include(g => g.Members)
            .FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);

        var group = await _context.Groups.FindAsync([groupId], cancellationToken);
        if (group is null)
        {
            return Result.Failure($"Group with id {groupId} doesn't exist.");
        }

        var user = await _context.Users.FindAsync([memberId], cancellationToken);
        if (user is null)
        {
            return Result.Failure($"User with id {memberId} doesn't exist.");
        }

        var newMember = new GroupMember(group, user);

        group.AddNewMember(newMember);

        return Result.Success();
    }
}
