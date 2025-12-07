using FluentValidation;
using IOU1.Application.Features.Users.Models.Endpoint;

namespace IOU1.Application.Features.Users;

public class AddUserValidator : AbstractValidator<AddUserRequest>
{
    public AddUserValidator()
    {
        RuleFor(au => au.FirstName)
            .NotEmpty()
            .WithErrorCode("USER_FIRST_NAME_ERROR")
            .WithMessage("First name cannot be null or empty.");

        RuleFor(au => au.LastName)
            .NotEmpty()
            .WithErrorCode("USER_LAST_NAME_ERROR")
            .WithMessage("Last name cannot be null or empty.");

        RuleFor(au => au.Email)
            .EmailAddress()
            .WithErrorCode("USER_EMAIL_ERROR")
            .WithMessage("Email address is not in correct format.");

        RuleFor(au => au.Login)
            .NotEmpty()
            .WithErrorCode("USER_LOGIN_ERROR")
            .WithMessage("Login cannot be null or empty.");

        RuleFor(au => au.Password)
            .NotEmpty()
            .WithErrorCode("USER_PASSWORD_ERROR")
            .WithMessage("Password cannot be null or empty.");
    }
}
