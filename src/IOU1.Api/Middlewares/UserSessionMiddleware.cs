using IOU1.Domain.Exceptions;
using IOU1.Domain.Models.Auth.User;

namespace IOU1.API.Middlewares;

public class UserSessionMiddleware(IAuthUser user) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        //If we keep proper attributes on the controller methods, we won't have situations where
        //the user is not authenticated, but the request is hitting a protected endpoint.
        //It's because the authorization middleware will block the request before it reaches this middleware.
        var isUserAuthenticated = context.User.Identity?.IsAuthenticated ?? false;
        if (!isUserAuthenticated)
            await next(context);
        else
        {
            SetUserSession(context);
            await next(context);
        }
    }

    private void SetUserSession(HttpContext context)
    {
        var userId =
            context.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value
            ?? throw new UserSessionException("Invalid user! ID is missing!");

        var userLogin =
            context.User.Claims.FirstOrDefault(c => c.Type == "login")?.Value
            ?? throw new UserSessionException("Invalid user! Login is missing!");

        if (!int.TryParse(userId, out var parsedUserId))
            throw new UserSessionException("Invalid user! ID is not a valid int value!");

        user.SetId(parsedUserId);
        user.SetLogin(userLogin);
    }
}
