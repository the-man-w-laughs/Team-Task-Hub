using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Extensions;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Commands;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, int>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ITaskModelRepository _taskModelRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly IProjectRepository _projectRepository;

    public CreateTaskCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ITaskModelRepository taskModelRepository,
        ITeamMemberRepository teamMemberRepository,
        IProjectRepository projectRepository
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _taskModelRepository = taskModelRepository;
        _teamMemberRepository = teamMemberRepository;
        _projectRepository = projectRepository;
    }

    public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.GetUserId();

        await _projectRepository.GetProjectByIdAsync(request.ProjectId);

        var teamMember = await _teamMemberRepository.GetTeamMemberAsync(userId, request.ProjectId);

        var taskToAdd = _mapper.Map<TaskModel>(request.TaskModelRequestDto);
        taskToAdd.ProjectId = request.ProjectId;
        taskToAdd.TeamMemberId = teamMember.Id;
        taskToAdd.CreatedAt = DateTime.Now;

        var addedComment = await _taskModelRepository.AddAsync(taskToAdd);
        await _taskModelRepository.SaveAsync();

        return addedComment.Id;
    }
}
