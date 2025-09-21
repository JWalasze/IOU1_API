using Domain.Entities;
using Domain.RepoInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IOU1Context _context;

    public UserRepository(IOU1Context context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<long> memberIds)
    {
        if (memberIds == null || !memberIds.Any())
            return Enumerable.Empty<User>();

        return await _context.Users
            .Where(u => memberIds.Contains(u.Id))
            .ToListAsync();
    }
}
