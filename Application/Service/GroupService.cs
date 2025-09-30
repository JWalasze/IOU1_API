using Application.Features.Groups.AddGroup.Dto;
using Application.Features.Groups.DeleteGroup.Dto;
using Domain.Entities;
using Domain.Models;
using Domain.RepoInterfaces;
using Domain.UnitOfWork;

namespace Application.Service;

public class GroupService(IUnitOfWork unit/*, ILogger logger*/) : IGroupService
{
    private readonly IUnitOfWork _unit = unit;
    //private readonly ILogger _logger = logger;

    public async Task<Result<AddedGroupDto?>> AddGroup(IEnumerable<long> memberIds, long ownerId, string description, CancellationToken cancellationToken = default)
    {
        if (!memberIds.Any())
            return Result<AddedGroupDto?>.Failure("Missing member ids.");

        try
        {
            await _unit.BeginTransaction();

            var userRepository = _unit.Get<IUserRepository>();
            var owner = await userRepository.GetByIdAsync(ownerId, cancellationToken);
            if (owner is null)
            {
                await _unit.RollbackTransaction();
                return Result<AddedGroupDto?>.Failure($"Couldn't find owner id: {ownerId}.");
            }

            var members = await userRepository.GetByIdsAsync(memberIds, cancellationToken);
            var foundMember = members.ToDictionary(m => m.Id, m => m);

            foreach (var memberId in memberIds)
            {
                //TODO: Result should have a property ErrorMessages
                var exists = foundMember.ContainsKey(memberId);
                if (!exists)
                {
                    await _unit.RollbackTransaction();
                    return Result<AddedGroupDto?>.Failure($"Couldn't find member id: {memberId}.");
                }
            }

            var groupRepository = _unit.Get<IGroupRepository>();
            var newGroup = new Group(description, owner);
            var newMembers = members.Select(member => new GroupMember(newGroup, member)).ToList();

            newGroup.AddNewMembers(newMembers);
            groupRepository.Add(newGroup);

            await _unit.SaveChanges(cancellationToken);
            await _unit.CommitTransaction();

            return Result<AddedGroupDto?>.Success(new AddedGroupDto(newGroup.Id));
        }
        catch (Exception ex)
        {
            //_logger.Error(ex);
            await _unit.RollbackTransaction();
            return Result<AddedGroupDto?>.Failure(ex.Message);
        }
    }

    public async Task<Result<DeleteGroupDto?>> DeleteGroup(long groupId, CancellationToken cancellationToken)
    {
        try
        {
            var groupRepository = _unit.Get<IGroupRepository>();

            await _unit.BeginTransaction();

            var group = await groupRepository.GetByIdAsync(groupId, cancellationToken);
            if (group is null)
            {
                await _unit.RollbackTransaction();
                return Result<DeleteGroupDto?>.Failure($"Group with id: {groupId} doesn't exist.");
            }

            groupRepository.Delete(group);

            await _unit.SaveChanges(cancellationToken);
            await _unit.CommitTransaction();

            return Result<DeleteGroupDto?>.Success(new DeleteGroupDto());
        }
        catch(Exception ex)
        {
            await _unit.RollbackTransaction();
            return Result<DeleteGroupDto?>.Failure(ex.Message);
        }
    }
}
