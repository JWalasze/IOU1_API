using IOU1.Application.Features.Users.AddUser.Models;
using IOU1.Domain.Entities;
using IOU1.Domain.Exceptions;
using IOU1.Domain.Models;
using IOU1.Domain.Services;
using IOU1.Domain.Services.Crypto;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IOU1.Application.Services.Users;

public class UserService(
    IOU1Context context,
    ILogger<UserService> logger,
    IPasswordHasher passwordHasher,
    IUserChecker userCheckerService)
    : IUserService
{
    private readonly IOU1Context _context = context;
    private readonly ILogger<UserService> _logger = logger;

    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUserChecker _userCheckerService = userCheckerService;

    public async Task<Result<User?>> Add(NewUser newUser, CancellationToken cancellationToken = default)
    {
        try
        {
            var isLoginTaken = await _userCheckerService.IsLoginTaken(newUser.Login);
            if (isLoginTaken)
            {
                return Result<User?>.Failure("The login is already taken.");
            }

            var isEmailTaken = await _userCheckerService.IsEmailTaken(newUser.Email);
            if (isEmailTaken)
            {
                return Result<User?>.Failure("The email is already taken.");
            }

            var user = User.Create(
                newUser.FirstName,
                newUser.LastName,
                new(newUser.Email),
                newUser.Login,
                newUser.Password,
                _passwordHasher);

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<User?>.Success(user);
        }
        catch (CreatingUserException ex)
        {
            _logger.LogError(ex, "An error occurred while adding a new user. User data: {@UserData}",
                new
                {
                    newUser.FirstName,
                    newUser.LastName,
                    newUser.Email,
                    newUser.Login
                });

            return Result<User?>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while adding a new user. User data: {@UserData}",
                new
                {
                    newUser.FirstName,
                    newUser.LastName,
                    newUser.Email,
                    newUser.Login
                });

            return Result<User?>.Failure($"An error occurred while adding the user.");
        }
    }

    public async Task<Result<string>> Delete(long userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.MemberGroups)
                .Include(u => u.OwnedGroups)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user is null)
            {
                return Result<string>.Failure("User does not exist.");
            }

            if (user.OwnedGroups.Count > 0)
            {
                return Result<string>.Failure("User cannot be deleted because they own one or more groups.");
            }

            user.Delete();
            user.MemberGroups.Clear();

            await _context.SaveChangesAsync(cancellationToken);

            return Result<string>.Success("User has been soft deleted.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting user with ID {UserId}.", userId);
            return Result<string>.Failure("An error occurred while deleting the user.");
        }
    }
}
