using IOU1.Application.Features.Users.DeleteUser.Models.Endpoint;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Users.DeleteUser;

public interface IDeleteUserHandler
{
    Task<Result<DeleteUserResponse?>> Handle(DeleteUserRequest request, CancellationToken cancellationToken = default);
}
