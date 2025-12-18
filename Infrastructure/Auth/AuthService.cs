using IOU1.Application.Features.Auth;
using IOU1.Domain.Models;
using IOU1.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace IOU1.Infrastructure.Auth;

public class AuthService(IOU1Context context) : IAuthService
{
    private readonly IOU1Context _context = context;

    public async Task<Result<Token>> LogIn(Credentials credentials)
    {
        try
        {
            //Seperate into methods?
            if (string.IsNullOrWhiteSpace(credentials.Login) || string.IsNullOrWhiteSpace(credentials.Password))
                throw new ArgumentException("Neither Password nor Login can be null or empty!");

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Login == credentials.Login)
                ?? throw new UserNotFoundException($"User with login {credentials.Login} couldn't be found.");

            //Into seperate service to be able to change implementation
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var hashedInputPassword = BCrypt.Net.BCrypt.HashPassword(credentials.Password, salt);
            var isVerified = BCrypt.Net.BCrypt.Verify(credentials.Password, hashedInputPassword);

            return Result<Token>.Success(new Token());
        }
        catch(Exception ex)
        {
            return Result<Token>.Failure(ex.Message);
        }
    }

    public class UserNotFoundException(string errorMessage) : Exception(errorMessage);
}
