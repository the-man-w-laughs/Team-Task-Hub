using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Commands;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskModelResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ITaskModelRepository _taskModelRepository;
    private readonly IProjectService _projectService;
    private readonly ITeamMemberService _teamMemberService;
    private readonly IUserService _userService;

    public CreateTaskCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ITaskModelRepository taskModelRepository,
        IProjectService projectService,
        ITeamMemberService teamMemberService,
        IUserService userService
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _taskModelRepository = taskModelRepository;
        _projectService = projectService;
        _teamMemberService = teamMemberService;
        _userService = userService;
    }

    public async Task<TaskModelResponseDto> Handle(
        CreateTaskCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        // Check if the user exists.
        await _userService.GetUserAsync(userId, cancellationToken);

        // get target project
        await _projectService.GetProjectAsync(request.ProjectId, cancellationToken);

        // only team members can create tasks
        var teamMember = await _teamMemberService.GetTeamMemberAsync(
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

        return result;
    }
}
