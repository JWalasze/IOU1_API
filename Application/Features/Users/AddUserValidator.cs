using FluentValidation;

namespace IOU1.Application.Features.Users;

public class AddUserValidator : AbstractValidator<AddUserRequest>
{
    public AddUserValidator()
    {
        RuleFor(au => au.FirstName)
            .NotEmpty()
            .WithMessage("First name cannot be null or empty.");

        RuleFor(au => au.LastName)
            .NotEmpty()
            .WithMessage("Last name cannot be null or empty.");

        RuleFor(au => au.Email)
            .EmailAddress()
            .WithMessage("Email address is not in correct format.");

        RuleFor(au => au.Login)
            .NotEmpty()
            .WithMessage("Login cannot be null or empty.");

        RuleFor(au => au.Password)
            .NotEmpty()
            .WithMessage("Password cannot be null or empty.");
    }
}
