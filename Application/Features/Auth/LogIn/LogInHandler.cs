using FluentValidation;
using FluentValidation.Results;
using IOU1.Application.Features.Auth.LogIn.Models;
using IOU1.Application.Mediator;
using IOU1.Domain.Interfaces;
using IOU1.Domain.Models;

namespace IOU1.Application.Features.Auth.LogIn;

public class LogInHandler(IValidator<LogInRequest> validator, IAuthService authService) : RequestHandler<LogInRequest, LogInResponse>(validator)
{
    private readonly IAuthService _authService = authService;

    protected override async Task<IResult> Do(LogInRequest request, CancellationToken cancellationToken = default)
    {
        var token = await _authService.LogIn(new(request.Login, request.Password));
        return Result.Success();
    }

    protected override LogInResponse MapFailure(IResult? result)
    {
        throw new NotImplementedException();
    }

    protected override LogInResponse MapFailureValidationResult(ValidationResult result)
    {
        throw new NotImplementedException();
    }

    protected override LogInResponse MapSuccess(IResult result)
    {
        throw new NotImplementedException();
    }
}
