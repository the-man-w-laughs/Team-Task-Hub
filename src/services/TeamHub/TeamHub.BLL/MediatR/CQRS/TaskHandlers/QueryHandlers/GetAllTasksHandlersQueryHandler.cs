using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.Extensions;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.TaskHandlers.Queries;

public class GetAllProjectsTeamMembersQueryHandler
    : IRequestHandler<GetAllTaskHandlersQuery, IEnumerable<UserResponseDto>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ITaskHandlerRepository _taskHandlerRepository;
    private readonly ITaskModelRepository _taskRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;

    public GetAllProjectsTeamMembersQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ITaskHandlerRepository taskHandlerRepository,
        ITaskModelRepository taskRepository,
        ITeamMemberRepository teamMemberRepository
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _taskHandlerRepository = taskHandlerRepository;
        _taskRepository = taskRepository;
        _teamMemberRepository = teamMemberRepository;
    }

    public async Task<IEnumerable<UserResponseDto>> Handle(
        GetAllTaskHandlersQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor.GetUserId();

        // Check if the requested task exists and current users team member exists.
        var task = await _taskRepository.GetByIdAsync(request.TaskId);

        if (task == null)
        {
            throw new NotFoundException($"Task with id {request.TaskId} was not found.");
        }

        var teamMember = await _teamMemberRepository.GetTeamMemberAsync(userId, task.ProjectId);

        if (teamMember == null)
        {
            throw new ForbiddenException(
                $"User with id {userId} doesn't have access to project with id {task.ProjectId}."
            );
        }

        var taskHandlers = await _taskHandlerRepository.GetAllAsync(
            teamMember => teamMember.TasksId == request.TaskId,
            request.Offset,
            request.Limit
        );

        var usersResponseDto = taskHandlers.Select(
            taskHandler => _mapper.Map<UserResponseDto>(taskHandler.TeamMember.User)
        );

        return usersResponseDto;
    }
}
