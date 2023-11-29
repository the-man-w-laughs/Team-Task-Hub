using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Dtos;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Commands;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskModelResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ITaskModelRepository _taskModelRepository;
    private readonly IProjectQueryService _projectService;
    private readonly ITeamMemberQueryService _teamMemberService;
    private readonly IUserQueryService _userService;
    private readonly ILogger<CreateTaskCommandHandler> _logger;

    public CreateTaskCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ITaskModelRepository taskModelRepository,
        IProjectQueryService projectService,
        ITeamMemberQueryService teamMemberService,
        IUserQueryService userService,
        ILogger<CreateTaskCommandHandler> logger
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _taskModelRepository = taskModelRepository;
        _projectService = projectService;
        _teamMemberService = teamMemberService;
        _userService = userService;
        _logger = logger;
    }

    public async Task<TaskModelResponseDto> Handle(
        CreateTaskCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        _logger.LogInformation(
            "User {UserId} is trying to create a new task for project {ProjectId}.",
            userId,
            request.ProjectId
        );

        // Check if the user exists.
        await _userService.GetExistingUserAsync(userId, cancellationToken);

        // get target project
        await _projectService.GetExistingProjectAsync(request.ProjectId, cancellationToken);

        // only team members can create tasks
        var teamMember = await _teamMemberService.GetExistingTeamMemberAsync(
            userId,
            request.ProjectId,
            cancellationToken
        );

        // create new task
        var taskToAdd = _mapper.Map<TaskModel>(request.TaskModelRequestDto);
        taskToAdd.ProjectId = request.ProjectId;
        taskToAdd.AuthorTeamMemberId = teamMember.Id;
        var addedTask = await _taskModelRepository.AddAsync(taskToAdd, cancellationToken);
        await _taskModelRepository.SaveAsync(cancellationToken);
        var result = _mapper.Map<TaskModelResponseDto>(addedTask);

        _logger.LogInformation(
            "Successfully created a new task with id {AddedTaskId} for project {ProjectId}.",
            addedTask.Id,
            request.ProjectId
        );

        return result;
    }
}
