using Application.Features.AddGroup.Dto;
using Application.Logging;
using Domain.Entities;
using Domain.RepoInterfaces;
using Domain.UnitOfWork;

namespace Application.Features.AddGroup.Service;

public class GroupService(IUnitOfWork unit, IGroupRepository groupRepository/*, ILogger logger*/) : IGroupService
{
    private readonly IGroupRepository _groupRepository = groupRepository;
    private readonly IUnitOfWork _unit = unit;
    //private readonly ILogger _logger = logger;

    public async Task<AddedGroupDto?> AddGroup(IEnumerable<long> memberIds)
    {
        if (!memberIds.Any())
            return null;

        try
        {
            await _unit.BeginTransaction();

            //var newGroup = new Group();

            await _unit.CommitTransaction();

            throw new Exception();
        }
        catch(Exception ex)
        {
            //_logger.Error(ex);
            await _unit.RollbackTransaction();

            return null;
        }
    }
}
