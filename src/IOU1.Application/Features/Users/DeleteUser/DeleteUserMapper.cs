using IOU1.Application.Features.Users.DeleteUser.Models.Endpoint;

namespace IOU1.Application.Features.Users.DeleteUser;

public static class DeleteUserMapper
{
    public static DeleteUserResponse ToDeleteUserResponse(this string message) => new(message);
}
