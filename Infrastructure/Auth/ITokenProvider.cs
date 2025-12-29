using IOU1.Domain.Entities;

namespace IOU1.Infrastructure.Auth;

public interface ITokenProvider
{
    string CreateToken(User user);
}
