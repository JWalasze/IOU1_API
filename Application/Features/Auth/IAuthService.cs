using IOU1.Domain.Models.Auth;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Auth;

public interface IAuthService
{
    Task<Result<Token?>> LogIn(Credentials credentials);
}
