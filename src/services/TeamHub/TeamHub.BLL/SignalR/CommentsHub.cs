using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Shared.Extensions;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.Extensions;
using TeamHub.BLL.MediatR.CQRS.Comments.Commands;
using MediatR;
using TeamHub.BLL.MediatR.CQRS.Tasks.Queries;

namespace TeamHub.BLL.SignalR
{
    [Authorize]
    public class CommentsHub : Hub<ICommentsHub>
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public CommentsHub(IConfiguration configuration, IMediator mediator)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User!.GetUserId();

            var taskId = ExtractTaskId();
            var groupId = GetGroupName(taskId);
            var connectionId = Context.ConnectionId;
            await Groups.AddToGroupAsync(connectionId, groupId);

            // check if user has access to the task
            var query = new GetTaskByIdQuery(taskId);
            var task = await _mediator.Send(query, Context.ConnectionAborted);

            Context.Items.SetTaskId(taskId);
            await Clients
                .GroupExcept(groupId, connectionId)
                .ConnectionAsync($"Client with id {userId} has connected to group {groupId}.");

            await Clients.Caller.ConnectionAsync(task);

            await base.OnConnectedAsync();
        }

        public async Task SendComment(CommentRequestDto commentRequestDto)
        {
            var taskId = Context.Items.GetTaskId();
            var command = new CreateCommentCommand(taskId, commentRequestDto);
            var result = await _mediator.Send(command, Context.ConnectionAborted);

            var groupName = GetGroupName(taskId);
            await Clients.Group(groupName).CreateCommentAsync(result);
        }

        public async Task UpdateComment(int commentId, CommentRequestDto commentRequestDto)
        {
            var taskId = Context.Items.GetTaskId();
            var command = new UpdateCommentCommand(commentId, commentRequestDto);
            var result = await _mediator.Send(command, Context.ConnectionAborted);

            var groupName = GetGroupName(taskId);
            await Clients.Group(groupName).UpdateCommentAsync(result);
        }

        public async Task DeleteComment(int commentId)
        {
            var command = new DeleteCommentCommand(commentId);
            var result = await _mediator.Send(command, Context.ConnectionAborted);

            var taskId = Context.Items.GetTaskId();
            var groupName = GetGroupName(taskId);
            await Clients.Group(groupName).DeleteCommentAsync(result);
        }

        private int ExtractTaskId()
        {
            var context = Context.GetHttpContext()!;
            var roomIdParameterName = _configuration["SignalR:GroupdParameterName"]!;

            var taskIdAsString = context.Request.Query[roomIdParameterName].ToString();

            if (int.TryParse(taskIdAsString, out var taskId))
            {
                return taskId;
            }
            else
            {
                throw new InvalidOperationException(
                    $"Unable to convert '{taskIdAsString}' to an integer."
                );
            }
        }

        private string GetGroupName(int taskId)
        {
            return $"task_{taskId}";
        }
    }
}
