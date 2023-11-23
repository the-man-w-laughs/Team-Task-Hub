using Microsoft.AspNetCore.Builder;
using TeamHub.BLL.SignalR;

namespace TeamHub.BLL.Extensions;

public static class SignalRRegistrationExtensions
{
    public static void UseSignalR(this WebApplication app)
    {
        var baseRoute = app.Configuration["SignalR:BaseRoute"];
        app.MapHub<CommentsHub>($"/{baseRoute}");
    }
}
