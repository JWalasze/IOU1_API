using FluentValidation;
using IOU1.Application.Features.Users.DeleteUser.Models.Endpoint;

namespace IOU1.Application.Features.Users.DeleteUser;

public class DeleteUserValidator : AbstractValidator<DeleteUserRequest>
{
    public DeleteUserValidator()
    {
        RuleFor(du => du.UserId)
            .GreaterThan(0)
            .WithErrorCode("USER_ID_ERROR")
            .WithMessage("User ID must be greater than zero.");
    }
}
