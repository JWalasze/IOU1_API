using IOU1.Application.Features.Auth;
using IOU1.Domain.Models.Auth;
using IOU1.Domain.Models.Results;
using IOU1.Domain.Services.Crypto;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IOU1.Infrastructure.Auth;

public class AuthService(
    IOU1Context context,
    ITokenProvider tokenProvider,
    IPasswordHasher passwordHasher,
    IPasswordComparer passwordComparer,
    ILogger<AuthService> logger) : IAuthService
{
    private readonly IOU1Context _context = context;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IPasswordComparer _passwordComparer = passwordComparer;
    private readonly ILogger<AuthService> _logger = logger;

    public async Task<Result<Token?>> LogIn(Credentials credentials)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(credentials.Login) || string.IsNullOrWhiteSpace(credentials.Password))
            {
                return Result<Token?>.Failure("Neither Password nor Login can be null or empty!");
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Login == credentials.Login);
            if (user is null)
            {
                return Result<Token?>.Failure($"User with login {credentials.Login} couldn't be found.");
            }

            var passwordHash = _passwordHasher.GenerateHash(credentials.Password, user.PasswordSalt);
            var compareResult = _passwordComparer.Compare(passwordHash, user.PasswordHash);
            if (!compareResult)
            {
                _logger.LogError("Invalid password for provided login: {login}.", credentials.Login);
                return Result<Token?>.Failure($"Invalid password for provided login: {credentials.Login}.");
            }

            var token = _tokenProvider.CreateToken(user);
            return Result<Token?>.Success(new(token));
        }
        catch (Exception ex)
        {
            return Result<Token?>.Failure(ex.Message);
        }
    }
}
