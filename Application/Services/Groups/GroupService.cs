using Application.Features.Groups.DeleteGroup.Dto;
using IOU1.Application.Features.Groups.AddGroup.Models;
using IOU1.Domain.Entities;
using IOU1.Domain.Exceptions;
using IOU1.Domain.Models.Auth.User;
using IOU1.Domain.Models.Results;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IOU1.Application.Services.Groups;

public sealed class GroupService(
    IOU1Context context,
    IAuthUser authUser,
    ILogger<GroupService> logger)
    : IGroupService
{
    private readonly IOU1Context _context = context;
    private readonly IAuthUser _authUser = authUser;
    private readonly ILogger<GroupService> _logger = logger;

    public async Task<Result<AddedGroup?>> Add(
        int ownerId,
        string name,
        string? description,
        string currencyKey,
        CancellationToken cancellationToken = default)
    {
        if (_authUser.Id != ownerId)
            return Result<AddedGroup?>.Failure($"Conflict between the logged user and the passed owner id.");

        try
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == ownerId, cancellationToken);
            if (user is null)
                return Result<AddedGroup?>.Failure($"User with id: {ownerId} doesn't exist.");

            var currency = await _context.Currencies
                .SingleOrDefaultAsync(c => c.Key == currencyKey, cancellationToken);
            if (currency is null)
                return Result<AddedGroup?>.Failure($"Currency {currencyKey} not found.");

            var newGroup = Group.Create(
                name,
                description,
                owner: user,
                currency,
                members: []);

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

    public async Task<Result<DeletedGroup?>> Delete(int groupId, CancellationToken cancellationToken)
    {
        try
        {
            var group = await _context.Groups
                .SingleOrDefaultAsync(g => g.Id == groupId, cancellationToken);

            if (group is null)
                return Result<DeletedGroup?>.Failure($"Group with id: {groupId} doesn't exist.");

            if (group.OwnerId != _authUser.Id)
                return Result<DeletedGroup?>.Failure($"User with id: {_authUser.Id} is not the owner of group with id: {groupId}.");

            //TODO: you can remove the group if every balance is settled. Maybe?

            _context.Remove(group);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<DeletedGroup?>.Success(new DeletedGroup());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured while deleting a group.");
            return Result<DeletedGroup?>.Failure("An unexpected error occured while deleting a group.");
        }
    }
}
