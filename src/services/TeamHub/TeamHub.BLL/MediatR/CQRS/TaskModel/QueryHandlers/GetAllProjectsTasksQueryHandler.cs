using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using Shared.Extensions;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Queries;

public class GetAllProjectsTasksQueryHandler
    : IRequestHandler<GetAllProjectsTasksQuery, IEnumerable<TaskModelResponseDto>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITaskModelRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly IMapper _mapper;

    public GetAllProjectsTasksQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        ITaskModelRepository taskTepository,
        IProjectRepository projectRepository,
        ITeamMemberRepository teamMemberRepository,
        IMapper mapper
    )
    {
        _taskRepository = taskTepository;
        _projectRepository = projectRepository;
        _teamMemberRepository = teamMemberRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<TaskModelResponseDto>> Handle(
        GetAllProjectsTasksQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor.GetUserId();

        await _projectRepository.GetProjectByIdAsync(request.ProjectId, cancellationToken);
        await _teamMemberRepository.GetTeamMemberAsync(
            userId,
            request.ProjectId,
            cancellationToken
        );

        var projectTasks = await _taskRepository.GetAllAsync(
            task => task.ProjectId == request.ProjectId,
            cancellationToken
        );

        var tasksResponseDtos = projectTasks.Select(
            project => _mapper.Map<TaskModelResponseDto>(project)
        );

        return tasksResponseDtos;
    }
}
