using IOU1.Domain.Entities;
using IOU1.Domain.RepoInterfaces;

namespace Domain.RepoInterfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByIdAsync(int userId, CancellationToken cancellation = default);
    Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<int> memberIds, CancellationToken cancellation = default);
}
