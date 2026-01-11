using Application.Features.Groups.DeleteGroup.Dto;
using IOU1.Application.Features.Groups.AddGroup.Models;
using IOU1.Domain.Entities;
using IOU1.Domain.Exceptions;
using IOU1.Domain.Models;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IOU1.Application.Services.Groups;

public class GroupService(
    IOU1Context context,
    ILogger<GroupService> logger)
    : IGroupService
{
    private readonly IOU1Context _context = context;
    private readonly ILogger<GroupService> _logger = logger;

    public async Task<Result<AddedGroup?>> AddGroup(
        IEnumerable<long> memberIds,
        long ownerId,
        string name,
        string? description,
        CancellationToken cancellationToken = default)
    {
        if (!memberIds.Any())
            return Result<AddedGroup?>.Failure("Missing member ids.");

        try
        {
            var membersWithOwnerIds = memberIds.Contains(ownerId) ?
                memberIds :
                [.. memberIds, ownerId];

            var users = await _context.Users
                .Where(u => membersWithOwnerIds.Contains(u.Id))
                .ToListAsync(cancellationToken);

            var missingUsersFromDB = users
                .Select(u => u.Id)
                .Except(membersWithOwnerIds)
                .ToList();

            if (missingUsersFromDB.Count > 0)
            {
                return Result<AddedGroup?>.Failure($"Could not find those user ids: {string.Concat(missingUsersFromDB)}");
            }

            var newGroup = Group.Create(
                name,
                description,
                owner: users.First(m => m.Id == ownerId),
                users: users);

            _context.Groups.Add(newGroup);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<AddedGroup?>.Success(new(newGroup.Id));
        }
        catch (CreateGroupException ex)
        {
            return Result<AddedGroup?>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured while adding a new group.");
            return Result<AddedGroup?>.Failure("An unexpected error occured while adding a new group.");
        }
    }

    public async Task<Result<DeleteGroupDto?>> DeleteGroup(long groupId, CancellationToken cancellationToken)
    {
        try
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                .SingleOrDefaultAsync(g => g.Id == groupId, cancellationToken);

            if (group is null)
            {
                return Result<DeleteGroupDto?>.Failure($"Group with id: {groupId} doesn't exist.");
            }

            _context.Remove(group);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<DeleteGroupDto?>.Success(new DeleteGroupDto());
        }
        catch (Exception ex)
        {
            return Result<DeleteGroupDto?>.Failure(ex.Message);
        }
    }
}
