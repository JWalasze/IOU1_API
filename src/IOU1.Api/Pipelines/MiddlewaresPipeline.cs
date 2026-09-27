using IOU1.API.Middlewares;

namespace IOU1.Api.Pipelines;

public static class MiddlewaresPipeline
{
    public static void AddMiddlewaresPipeline(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<UserSessionMiddleware>();
    }
}
