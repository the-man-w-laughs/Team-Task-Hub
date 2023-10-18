using System.Net;
using Microsoft.IdentityModel.Tokens;
using Shared.Exceptions;

namespace TeamHub.WebApi.Middleweres;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        // try
        // {
        //     await _next(httpContext);
        // }
        // catch (Exception ex)
        // {
        //     await HandleExceptionAsync(httpContext, ex);
        // }
        await _next(httpContext);
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        switch (exception)
        {
            case AuthorizationException:
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                var message = exception.Message.IsNullOrEmpty()
                    ? "Access denied"
                    : exception.Message;
                await context.Response.WriteAsync(message);
                break;
            }
            case CustomValidationException:
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var message = exception.Message.IsNullOrEmpty()
                    ? "Validation error"
                    : exception.Message;
                await context.Response.WriteAsync(message);
                break;
            }
            default:
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync("Internal server error");
                break;
            }
        }
    }
}
