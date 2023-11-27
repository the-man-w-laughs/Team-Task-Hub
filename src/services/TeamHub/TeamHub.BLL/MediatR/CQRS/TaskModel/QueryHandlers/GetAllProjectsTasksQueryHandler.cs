using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using Shared.Extensions;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.BLL.Contracts;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Queries;

public class GetAllProjectsTasksQueryHandler
    : IRequestHandler<GetAllProjectsTasksQuery, IEnumerable<TaskModelResponseDto>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITaskModelRepository _taskRepository;
    private readonly IMapper _mapper;
    private readonly IUserQueryService _userService;
    private readonly IProjectQueryService _projectService;
    private readonly ITeamMemberQueryService _teamMemberService;

    public GetAllProjectsTasksQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        ITaskModelRepository taskTepository,
        IMapper mapper,
        IUserQueryService userService,
        IProjectQueryService projectService,
        ITeamMemberQueryService teamMemberService
    )
    {
        _taskRepository = taskTepository;
        _mapper = mapper;
        _userService = userService;
        _projectService = projectService;
        _teamMemberService = teamMemberService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<TaskModelResponseDto>> Handle(
        GetAllProjectsTasksQuery request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        // Check if the user exists.
        await _userService.GetExistingUserAsync(userId, cancellationToken);

        // get required project
        var project = await _projectService.GetExistingProjectAsync(
            request.ProjectId,
            cancellationToken
        );

        // only team member has access to related entities
        var teamMember = await _teamMemberService.GetExistingTeamMemberAsync(
            userId,
            request.ProjectId,
            cancellationToken
        );

        // get all project related tasks
        var projectTasks = await _taskRepository.GetAllAsync(
            task => task.ProjectId == request.ProjectId,
            request.Offset,
            request.Limit,
            cancellationToken
        );
        var tasksResponseDtos = projectTasks.Select(
            project => _mapper.Map<TaskModelResponseDto>(project)
        );

        return tasksResponseDtos;
    }
}
