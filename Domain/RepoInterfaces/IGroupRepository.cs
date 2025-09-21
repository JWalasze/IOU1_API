using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepoInterfaces;
public interface IGroupRepository : IRepository<Group>
{
    Task<Group?> GetByIdAsync(long groupId);
    Task<Group?> GetGroupWithMembersAsync(long groupId);
}


