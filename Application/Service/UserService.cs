using IOU1.Application.Features.Users.AddUser.Models;
using IOU1.Domain.Entities;
using IOU1.Domain.Exceptions;
using IOU1.Domain.Models;
using IOU1.Domain.Services;
using IOU1.Persistance.Context;
using Microsoft.Extensions.Logging;

namespace IOU1.Application.Service;

public class UserService(IOU1Context context, ILogger<UserService> logger, IPasswordHasher passwordHasher, IUserChecker userCheckerService) : IUserService
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

            var user = new User(
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
        catch(CreatingUserException ex)
        {
            _logger.LogError(ex, "An error occurred while adding a new user. User data: {UserData}", @newUser);
            return Result<User?>.Failure(ex.Message);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while adding a new user. User data: {UserData}", @newUser);
            return Result<User?>.Failure($"An error occurred while adding the user.");
        }
    }
}
