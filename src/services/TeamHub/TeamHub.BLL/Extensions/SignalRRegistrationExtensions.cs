using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Shared.Exceptions;
using TeamHub.BLL.SignalR;

namespace TeamHub.BLL.Extensions;

public static class SignalRRegistrationExtensions
{
    public static void UseSignalR(this WebApplication app)
    {
        var commentsRoute = app.Configuration["SignalR:CommentsRoute"];
        var baseRoute = app.Configuration["SignalR:BaseRoute"];
        app.MapHub<CommentsHub>($"/{baseRoute}/{commentsRoute}");
    }
}
