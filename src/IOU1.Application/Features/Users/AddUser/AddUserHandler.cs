using FluentValidation;
using IOU1.Application.Features.Users.AddUser.Models.Endpoint;
using IOU1.Application.Services.Users;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Users.AddUser;

public sealed class AddUserHandler(IUserService userService, IValidator<AddUserRequest> validator)
    : IAddUserHandler
{
    private readonly IUserService _userService = userService;
    private readonly IValidator<AddUserRequest> _validator = validator;

    public async Task<Result<AddUserResponse?>> Handle(
        AddUserRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Result<AddUserResponse?>.Failure(
                validationResult.Errors.Select(e => new ProblemDetails(e.ErrorMessage, e.ErrorCode))
            );
        }

        var result = await _userService.Add(
            new(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Login,
                request.Password
            ),
            cancellationToken
        );

        if (!result.IsSuccess || result.Data is null)
        {
            return Result<AddUserResponse?>.Failure(
                result.ErrorMessage ?? "Unexpected error occured!"
            );
        }

        return Result<AddUserResponse?>.Success(result.ToAddUserResponse());
    }
}
