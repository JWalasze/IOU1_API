using IOU1.Application.Features.Users.AddUser.Models.Endpoint;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Users.AddUser;

public interface IAddUserHandler
{
    Task<Result<AddUserResponse?>> Handle(AddUserRequest request, CancellationToken cancellationToken = default);
}
