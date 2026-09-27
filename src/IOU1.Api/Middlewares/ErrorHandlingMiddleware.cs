using IOU1.Domain.Exceptions;
using IOU1.Domain.Models.Results;
using System.Net;
using System.Text.Json;

namespace IOU1.API.Middlewares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception while processing {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteErrorResponse(context, ex);
        }
    }

    private static Task WriteErrorResponse(HttpContext context, Exception ex)
    {
        var statusCode = ex switch
        {
            UserSessionException => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError
        };

        var errors = new[] { new ProblemDetails(ex.GetType().Name, ex.Message) };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var payload = JsonSerializer.Serialize(new { isSuccess = false, errors });

        return context.Response.WriteAsync(payload);
    }
}
