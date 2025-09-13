using Domain.Entities;
using Domain.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOU1_API.Services;
public class GroupService
{
    private readonly IGroupRepository _groupRepository;

    public GroupService(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<Group?> GetGroupWithMembersAsync(long groupId)
    {
        return await _groupRepository.GetGroupWithMembersAsync(groupId);
    }
}
