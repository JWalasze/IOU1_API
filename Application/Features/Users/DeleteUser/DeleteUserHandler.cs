using FluentValidation;
using IOU1.Application.Features.Users.DeleteUser.Models.Endpoint;
using IOU1.Application.Mediator;
using IOU1.Application.Services.Users;
using IOU1.Domain.Interfaces;
using MapsterMapper;

namespace IOU1.Application.Features.Users.DeleteUser;

public sealed class DeleteUserHandler(
    IUserService userService,
    IValidator<DeleteUserRequest> validator,
    IMapper mapper)
    : RequestHandler<DeleteUserRequest, DeleteUserResponse>(validator, mapper)
{
    private readonly IUserService _userService = userService;

    protected override async Task<IResult> Do(DeleteUserRequest request, CancellationToken cancellationToken = default)
    {
        return await _userService.Delete(
            request.UserId,
            cancellationToken);
    }
}
