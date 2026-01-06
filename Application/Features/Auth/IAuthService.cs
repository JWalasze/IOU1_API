using IOU1.Domain.Models;

namespace IOU1.Application.Features.Auth;

public interface IAuthService
{
    Task<Result<Token?>> LogIn(Credentials credentials);
}
