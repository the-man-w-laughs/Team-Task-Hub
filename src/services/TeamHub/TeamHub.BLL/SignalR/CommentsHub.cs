using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared.Extensions;
using TeamHub.BLL.Contracts;

namespace TeamHub.BLL.SignalR
{
    [Authorize]
    public class CommentsHub : Hub
    {
        public CommentsHub() { }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User!.GetUserId();

            await Clients.All.SendAsync("connection", $"Client with id {userId} has connected.");

            await base.OnConnectedAsync();
        }

        public async Task SendComment(string comment)
        {
            var userId = Context.User!.GetUserId();

            await Clients.All.SendAsync("comment", $"{userId}: {comment}");
        }
    }
}
