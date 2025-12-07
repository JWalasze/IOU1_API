using FluentValidation;
using IOU1.Application.Features.Users.Models.Endpoint;
using IOU1.Application.Mediator;
using IOU1.Application.Service;
using IOU1.Domain.Entities;
using IOU1.Domain.Interfaces;

namespace IOU1.Application.Features.Users;

public class AddUserHandler(IValidator<AddUserRequest> validator, IUserService userService) : RequestHandler<AddUserRequest, AddUserResponse>(validator)
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
        var testMaper = new UserMapper();
        return testMaper.Map<AddUserResponse>(result);
    }

    protected override AddUserResponse MapSuccess(IResult result)
    {
        var testMaper = new UserMapper();
        return testMaper.Map<AddUserResponse>(((IResult<User>)result).Data); 
    }
}
