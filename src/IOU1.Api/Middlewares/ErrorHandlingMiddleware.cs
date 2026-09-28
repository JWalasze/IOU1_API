using System.Net;
using IOU1.Domain.Exceptions;
using IOU1.Domain.Models.Results;
using Microsoft.AspNetCore.WebUtilities;

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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var statusCode = ResolveStatusCode(ex);

        logger.LogError(
            ex,
            "Unhandled exception occurred while processing {Method} {Path}",
            context.Request.Method,
            context.Request.Path
        );

        var problem = new Problem
        {
            Title = ReasonPhrases.GetReasonPhrase((int)statusCode),
            Description = ex.Message,
            StatusCode = ((int)statusCode).ToString(),
            Errors = [new ProblemItem { Code = ex.GetType().Name, Message = ex.Message }],
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsJsonAsync(problem);
    }

    private static HttpStatusCode ResolveStatusCode(Exception ex) =>
        ex switch
        {
            UserSessionException => HttpStatusCode.Unauthorized,
            UserNotFoundException => HttpStatusCode.NotFound,
            CreatingUserException
            or CreateGroupException
            or CreatingExpenseException
            or CreatingExpenseCategoryException
            or DeletingExpenseCategoryException
            or CreatingExpenseShareException
            or CreatingExpenseSplitException
            or DeletingExpenseSplitException
            or CreatingInvitationException
            or CreatingMemberBalanceException
            or CreatingNotificationException
            or InvitationStatusException
            or InvalidEntityStateException
            or InvalidResultState => HttpStatusCode.UnprocessableEntity,
            _ => HttpStatusCode.InternalServerError,
        };
}
