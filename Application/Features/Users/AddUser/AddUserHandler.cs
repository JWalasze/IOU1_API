using FluentValidation;
using IOU1.Application.Features.Users.AddUser.Models.Endpoint;
using IOU1.Application.Mediator;
using IOU1.Application.Service;
using IOU1.Domain.Interfaces;
using Mapster;
using MapsterMapper;

namespace IOU1.Application.Features.Users.AddUser;

public class AddUserHandler(IValidator<AddUserRequest> validator, IUserService userService, IMapper mapper) : RequestHandler<AddUserRequest, AddUserResponse>(validator, mapper)
{
    private readonly IUserService _userService = userService;

    protected override async Task<IResult> Do(AddUserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _userService.Add(new(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Login,
            request.Password), cancellationToken);

        return result;
    }

    protected override AddUserResponse MapFailure(IResult? result)
    {
        return result.Adapt<AddUserResponse>();
    }

    protected override AddUserResponse MapSuccess(IResult result)
    {
        return result.Adapt<AddUserResponse>();
    }
}
