using Domain.Entities;
using Domain.RepoInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository, IRepository<User>
{
    private readonly IOU1Context _context;

    public UserRepository(IOU1Context context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(long userId, CancellationToken cancellation = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<long> memberIds, CancellationToken cancellation = default)
    {
        if (memberIds is null || !memberIds.Any())
            return [];

        return await _context.Users
            .Where(u => memberIds.Contains(u.Id))
            .ToListAsync(cancellation);
    }
}
