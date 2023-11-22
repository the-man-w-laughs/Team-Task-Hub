using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Dtos.TaskHandler;

namespace TeamHub.BLL.MediatR.CQRS.TaskHandlers.Commands;

public class DeleteTaskHandlerCommandHandler
    : IRequestHandler<DeleteTaskHandlerCommand, TaskHandlerResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ITeamMemberService _teamMemberService;
    private readonly IUserService _userService;
    private readonly ITaskHandlerService _taskHandlerService;
    private readonly ITaskService _taskService;

    public DeleteTaskHandlerCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ITeamMemberService teamMemberService,
        IUserService userService,
        ITaskHandlerService taskHandlerService,
        ITaskService taskService
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _teamMemberService = teamMemberService;
        _userService = userService;
        _taskHandlerService = taskHandlerService;
        _taskService = taskService;
    }

    public async Task<TaskHandlerResponseDto> Handle(
        DeleteTaskHandlerCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        // Check if the current user exists.
        await _userService.GetUserAsync(userId, cancellationToken);
        // Check if the target user exists.
        await _userService.GetUserAsync(request.UserId, cancellationToken);

        // Check if the task exists.
        var task = await _taskService.GetTaskAsync(request.TaskId, cancellationToken);

        // Check if the current users team member exists.
        await _teamMemberService.GetTeamMemberAsync(userId, task.ProjectId, cancellationToken);

        // Check if the target team member exists (the user to be assigned).
        var targetTeamMember = await _teamMemberService.GetTeamMemberAsync(
            request.UserId,
            task.ProjectId,
            cancellationToken
        );

        // remove task handler
        var taskHandler = await _taskHandlerService.RemoveTaskHandlerAsync(
            targetTeamMember,
            request.TaskId,
            cancellationToken
        );
        var result = _mapper.Map<TaskHandlerResponseDto>(taskHandler);

        return result;
    }
}
