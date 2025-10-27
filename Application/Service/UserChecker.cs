using IOU1.Domain.Services;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Application.Service;

public class UserChecker(IOU1Context context) : IUserChecker
{
    private readonly IOU1Context _context = context;

    public async Task<bool> IsLoginTaken(string login)
    {
        return await _context.Users.AnyAsync(u => u.Login == login);
    }

    public async Task<bool> IsEmailTaken(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email.EmailAddress == email);
    }
}
