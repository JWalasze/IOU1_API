using FluentValidation;
using IOU1.Application.Features.Users.DeleteUser.Models.Endpoint;
using IOU1.Application.Services.Users;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Users.DeleteUser;

public sealed class DeleteUserHandler(
    IUserService userService,
    IValidator<DeleteUserRequest> validator)
    : IDeleteUserHandler
{
    private readonly IUserService _userService = userService;
    private readonly IValidator<DeleteUserRequest> _validator = validator;

    public async Task<Result<DeleteUserResponse?>> Handle(DeleteUserRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Result<DeleteUserResponse?>.Failure(
                validationResult.Errors.Select(e => new ProblemDetails(e.ErrorMessage, e.ErrorCode)));
        }

        var result = await _userService.Delete(request.UserId, cancellationToken);
        if (!result.IsSuccess || result.Data is null)
        {
            return Result<DeleteUserResponse?>.Failure(result.ErrorMessage ?? "Unexpected error occured!");
        }

        return Result<DeleteUserResponse?>.Success(result.Data.ToDeleteUserResponse());
    }
}
