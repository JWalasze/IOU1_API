using IOU1.Application.Features.Auth.LogIn.Models;
using IOU1.Domain.Models.Auth;
using IOU1.Domain.Models.Results;

namespace IOU1.Application.Features.Auth.LogIn;

public static class LogInMapper
{
    public static LogInResponse ToResponse(this Result<Token?> result) =>
        new() { Token = result.Data?.Value };
}
