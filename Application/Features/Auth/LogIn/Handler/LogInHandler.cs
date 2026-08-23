using FluentValidation;
using IOU1.Application.Features.Auth.LogIn.Models;
using IOU1.Domain.Models.Results;
using MapsterMapper;

namespace IOU1.Application.Features.Auth.LogIn.Handler;

public class LogInHandler(
    IValidator<LogInRequest> validator,
    IMapper mapper,
    IAuthService authService)
    : ILogInHandler
{
    private readonly IValidator<LogInRequest> _validator = validator;
    private readonly IMapper _mapper = mapper;
    private readonly IAuthService _authService = authService;

    public async Task<Result<LogInResponse>> Handle(LogInRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
            return Result<LogInResponse>.Failure(validationResult.Errors);

        var logInResult = await _authService.LogIn(new(request.Login, request.Password));
        if (!logInResult)
        {
            var errorMessage = logInResult.ErrorMessage ?? "Error occured while logging in.";
            return Result<LogInResponse>.Failure("AUTH_LOG_IN_ERROR", errorMessage);
        }

        var result = _mapper.Map<LogInResponse>(logInResult);
        return Result<LogInResponse>.Success(result);
    }
}
