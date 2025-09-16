using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepoInterfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(long creatorId);
    Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<long> memberIds);
}
