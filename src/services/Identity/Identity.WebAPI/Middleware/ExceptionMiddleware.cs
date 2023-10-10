using Microsoft.AspNetCore.Http;
using System.Net;

namespace Identity.WebAPI.Middleware;
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;    
    private const string ContentType = "application/json";
    private const string Status500ErrorMessage = "Internal server error";

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;        
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {        
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = ContentType;
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync(exception.Message);
    }
}