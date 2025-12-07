using IOU1.Application.Features.Users.Models;
using IOU1.Domain.Entities;
using IOU1.Domain.Models;

namespace IOU1.Application.Service;

public interface IUserService
{
    Task<Result<User?>> Add(NewUser newUser, CancellationToken cancellationToken = default);
}
