using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Shared.Extensions;
using TeamHub.BLL.Contracts;

namespace TeamHub.BLL.SignalR
{
    [Authorize]
    public class CommentsHub : Hub<ICommentsHub>
    {
        private readonly IConfiguration _configuration;

        public CommentsHub(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User!.GetUserId();

            var groupId = ExtractGroupId();
            var connectionId = Context.ConnectionId;
            await Groups.AddToGroupAsync(connectionId, groupId);

            await Clients
                .Group(groupId)
                .Connection($"Client with id {userId} has connected to group {groupId}.");

            await base.OnConnectedAsync();
        }

        private string ExtractGroupId()
        {
            var context = Context.GetHttpContext()!;
            var roomIdParameterName = _configuration["GroupdParameterName"]!;
            var roomId = context.Request.Query[roomIdParameterName].ToString();
            return $"task_{roomId}";
        }

        public async Task SendComment(string comment)
        {
            var userId = Context.User!.GetUserId();

            await Clients.All.NewComment($"{userId}: {comment}");
        }
    }
}
