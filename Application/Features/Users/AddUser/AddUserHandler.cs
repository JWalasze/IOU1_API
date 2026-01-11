using FluentValidation;
using IOU1.Application.Features.Users.AddUser.Models.Endpoint;
using IOU1.Application.Mediator;
using IOU1.Application.Services.Users;
using IOU1.Domain.Interfaces;
using MapsterMapper;

namespace IOU1.Application.Features.Users.AddUser;

public sealed class AddUserHandler(
    IUserService userService,
    IValidator<AddUserRequest> validator,
    IMapper mapper)
    : RequestHandler<AddUserRequest, AddUserResponse>(validator, mapper)
{
    private readonly IUserService _userService = userService;

    protected override async Task<IResult> Do(AddUserRequest request, CancellationToken cancellationToken = default)
    {
        return await _userService.Add(new(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Login,
            request.Password), cancellationToken);
    }
}
