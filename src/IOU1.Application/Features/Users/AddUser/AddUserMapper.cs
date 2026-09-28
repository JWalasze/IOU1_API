using IOU1.Application.Features.Users.AddUser.Models.Endpoint;
using IOU1.Domain.Entities;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Users.AddUser;

public static class AddUserMapper
{
    public static AddUserResponse ToAddUserResponse(this Result<User?> result)
    {
        var user = result.Data!;

        return new AddUserResponse(
            user.FirstName,
            user.LastName,
            user.Email.EmailAddress,
            user.Login
        )
        {
            ErrorMessage = result.ErrorMessage,
            Errors = result.Errors,
        };
    }
}
