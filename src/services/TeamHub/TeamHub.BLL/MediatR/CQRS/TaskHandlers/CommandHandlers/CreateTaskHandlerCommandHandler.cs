using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using Shared.Extensions;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Dtos.TaskHandler;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.MediatR.CQRS.TaskHandlers.Commands;

public class CreateTaskHandlerCommandHandler
    : IRequestHandler<CreateTaskHandlerCommand, TaskHandlerResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITeamMemberQueryService _teamMemberService;
    private readonly IUserQueryService _userService;
    private readonly IMapper _mapper;
    private readonly ITaskQueryService _taskService;
    private readonly ITaskHandlerRepository _taskHandlerRepository;

    public CreateTaskHandlerCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        ITeamMemberQueryService teamMemberService,
        IUserQueryService userService,
        IMapper mapper,
        ITaskQueryService taskService,
        ITaskHandlerRepository taskHandlerRepository
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _teamMemberService = teamMemberService;
        _userService = userService;
        _mapper = mapper;
        _taskService = taskService;
        _taskHandlerRepository = taskHandlerRepository;
    }

    public async Task<TaskHandlerResponseDto> Handle(
        CreateTaskHandlerCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        // Check if the current user exists.
        await _userService.GetExistingUserAsync(userId, cancellationToken);
        // Check if the target user exists.
        await _userService.GetExistingUserAsync(request.UserId, cancellationToken);

        // Check if the task exists.
        var task = await _taskService.GetExistingTaskAsync(request.TaskId, cancellationToken);

        // Check if current user has access to the project.
        var projectId = task.ProjectId;
        await _teamMemberService.GetExistingTeamMemberAsync(userId, projectId, cancellationToken);

        // Check if target user has access to the project.
        var targetTeamMember = await _teamMemberService.GetExistingTeamMemberAsync(
            request.UserId,
            projectId,
            cancellationToken
        );

        // Check if the task handler already exists.
        var taskHandler = await _taskHandlerRepository.GetTaskHandlerAsync(
            targetTeamMember.Id,
            request.TaskId,
            cancellationToken
        );

        if (taskHandler != null)
        {
            throw new WrongActionException(
                $"User with id {request.UserId} is already assigned to the task with Id {request.TaskId}."
            );
        }

        // Create a new TaskHandler entity and add it to the repository.
        var taskHandlerToAdd = new TaskHandler()
        {
            TaskId = request.TaskId,
            TeamMemberId = targetTeamMember.Id,
            CreatedAt = DateTime.Now
        };

        var addedTaskHandler = await _taskHandlerRepository.AddAsync(
            taskHandlerToAdd,
            cancellationToken
        );
        await _taskHandlerRepository.SaveAsync(cancellationToken);

        var result = _mapper.Map<TaskHandlerResponseDto>(addedTaskHandler);

        return result;
    }
}
