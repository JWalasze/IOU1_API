
using IOU1.Domain.Models;

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
            var userId = context.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value
                ?? throw new InvalidOperationException("Invalid user! ID is missing!");

            _authUser.Id = long.Parse(userId);

            await next(context);
        }
    }
}
