using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Dtos.TaskHandler;

namespace TeamHub.BLL.MediatR.CQRS.TaskHandlers.Commands;

public class CreateTaskHandlerCommandHandler
    : IRequestHandler<CreateTaskHandlerCommand, TaskHandlerResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITeamMemberService _teamMemberService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly ITaskHandlerService _taskHandlerService;
    private readonly ITaskService _taskService;

    public CreateTaskHandlerCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        ITeamMemberService teamMemberService,
        IUserService userService,
        IMapper mapper,
        ITaskHandlerService taskHandlerService,
        ITaskService taskService
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _teamMemberService = teamMemberService;
        _userService = userService;
        _mapper = mapper;
        _taskHandlerService = taskHandlerService;
        _taskService = taskService;
    }

    public async Task<TaskHandlerResponseDto> Handle(
        CreateTaskHandlerCommand request,
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

        // Check if current user has access to the project.
        var projectId = task.ProjectId;
        await _teamMemberService.GetTeamMemberAsync(userId, projectId, cancellationToken);

        // Check if target user has access to the project.
        var targetTeamMember = await _teamMemberService.GetTeamMemberAsync(
            request.UserId,
            projectId,
            cancellationToken
        );

        // Create task handler
        var addedTaskHandler = await _taskHandlerService.AddTaskHandlerAsync(
            targetTeamMember,
            request.TaskId,
            cancellationToken
        );
        var result = _mapper.Map<TaskHandlerResponseDto>(addedTaskHandler);

        return result;
    }
}
