using IOU1.Domain.Entities;
using IOU1.Domain.Models.Results;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Services.Members;

public class MemberService(IOU1Context context) : IMemberService
{
    private readonly IOU1Context _context = context;

    public async Task<Result<GroupMember?>> AddMember(int groupId, int memberId, CancellationToken cancellationToken = default)
    {
        var group = await _context.Groups.FindAsync([groupId], cancellationToken);
        if (group is null)
        {
            return Result<GroupMember?>.Failure($"Group with id {groupId} doesn't exist.");
        }

        var user = await _context.Users.FindAsync([memberId], cancellationToken);
        if (user is null)
        {
            return Result<GroupMember?>.Failure($"User with id {memberId} doesn't exist.");
        }

        if (group.Members
                .Select(m => m.UserId)
                .Contains(user.Id))
        {
            var existingMember = await _context.GroupMembers
                .FirstOrDefaultAsync(gm =>
                    gm.GroupId == groupId &&
                    gm.UserId == user.Id,
                    cancellationToken);

            if (existingMember is null)
                return Result<GroupMember?>.Failure($"Inconsistent data around user {user.Id} in group {group.Id}.");

            return Result<GroupMember?>.Success(existingMember);
        }

        var newMember = new GroupMember(group, user);
        group.AddNewMember(newMember);

        return Result<GroupMember?>.Success(newMember);
    }

    public Task<bool> IsMemberOfGroup(int groupId, int memberId, CancellationToken cancellationToken = default)
    {
        return _context
            .GroupMembers
            .AnyAsync(gm => gm.UserId == memberId && gm.GroupId == groupId, cancellationToken);
    }
}
