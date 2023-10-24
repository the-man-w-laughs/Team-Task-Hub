using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using Shared.Extensions;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Queries;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskModelResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectRepository _projectRepository;
    private readonly ITaskModelRepository _taskRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly IMapper _mapper;

    public GetTaskByIdQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IProjectRepository projectRepository,
        ITaskModelRepository taskRepository,
        ITeamMemberRepository teamMemberRepository,
        IMapper mapper
    )
    {
        _taskRepository = taskRepository;
        _teamMemberRepository = teamMemberRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _projectRepository = projectRepository;
    }

    public async Task<TaskModelResponseDto> Handle(
        GetTaskByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor.GetUserId();

        var task = await _taskRepository.GetTaskByIdAsync(request.TaskId);

        await _projectRepository.GetProjectByIdAsync(task.ProjectId);
        await _teamMemberRepository.GetTeamMemberAsync(userId, task.ProjectId);

        var response = _mapper.Map<TaskModelResponseDto>(task);

        return response;
    }
}
