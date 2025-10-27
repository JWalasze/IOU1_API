using FluentValidation;
using IOU1.Application.Features.Auth.LogIn.Models;

namespace IOU1.Application.Features.Auth.LogIn;

public class LogInValidator : AbstractValidator<LogInRequest>
{
    public LogInValidator()
    {
        RuleFor(l => l.Login)
            .NotEmpty()
            .WithMessage("Login cannot be null or empty.");

        RuleFor(l => l.Password)
            .NotEmpty()
            .WithMessage("Password cannot be null or empty.");
    }
}
