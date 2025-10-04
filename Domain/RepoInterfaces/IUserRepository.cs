using Domain.Entities;
using IOU1.Domain.RepoInterfaces;

namespace Domain.RepoInterfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByIdAsync(long userId, CancellationToken cancellation = default);
    Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<long> memberIds, CancellationToken cancellation = default);
}
