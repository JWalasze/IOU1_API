using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepoInterfaces;
public interface IGroupRepository
{
    Task<Group?> GetGroupWithMembersAsync(long groupId);
}


