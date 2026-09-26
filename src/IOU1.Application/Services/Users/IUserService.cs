using IOU1.Application.Features.Users.AddUser.Models;
using IOU1.Domain.Entities;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Services.Users;

public interface IUserService
{
    Task<Result<User?>> Add(NewUser newUser, CancellationToken cancellationToken = default);

    Task<Result<string>> Delete(int userId, CancellationToken cancellationToken = default);
}
