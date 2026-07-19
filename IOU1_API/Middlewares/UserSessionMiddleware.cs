using IOU1.Domain.Exceptions;
using IOU1.Domain.Models.Auth.User;

namespace IOU1.API.Middlewares;

public class UserSessionMiddleware(IAuthUser user) : IMiddleware
{
    private readonly IAuthUser _authUser = user;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var isUserAuthenticated = context.User.Identity?.IsAuthenticated ?? false;
        if (!isUserAuthenticated)
        {
            await next(context);
        }
        else
        {
            SetUserSession(context);
            await next(context);
        }
    }

    private void SetUserSession(HttpContext context)
    {
        var userId = context.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value ??
            throw new UserSessionException("Invalid user! ID is missing!");

        if (!int.TryParse(userId, out var parsedUserId))
            throw new UserSessionException("Invalid user! ID is not a valid int value!");

        _authUser.Id = parsedUserId;
    }
}
