using IOU1.Application.Features.Auth.LogIn.Models;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Auth.LogIn.Handler;

public interface ILogInHandler
{
    Task<Result<LogInResponse>> Handle(LogInRequest request, CancellationToken cancellationToken = default);
}
