using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.Extensions;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.TeamMembers.Queries;

public class GetAllProjectsTeamMembersQueryHandler
    : IRequestHandler<GetAllProjectsTeamMembersQuery, IEnumerable<UserResponseDto>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectRepository _projectRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly IMapper _mapper;

    public GetAllProjectsTeamMembersQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IProjectRepository projectRepository,
        ITeamMemberRepository teamMemberRepository,
        IMapper mapper
    )
    {
        _projectRepository = projectRepository;
        _teamMemberRepository = teamMemberRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<UserResponseDto>> Handle(
        GetAllProjectsTeamMembersQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();

        var project = await _projectRepository.GetAsync(project => project.Id == request.ProjectId);

        if (project == null)
        {
            throw new NotFoundException($"Project with id {request.ProjectId} is not found.");
        }

        var teamMember = await _teamMemberRepository.GetAsync(
            teamMember => teamMember.UserId == userId && teamMember.ProjectId == request.ProjectId
        );

        if (teamMember == null)
        {
            throw new WrongActionException(
                $"User with id {userId} is not a team member of project with id {request.ProjectId}."
            );
        }

        var teamMembers = await _teamMemberRepository.GetAllAsync(
            teamMember => teamMember.ProjectId == request.ProjectId
        );

        var usersResponseDto = teamMembers.Select(
            user => _mapper.Map<UserResponseDto>(teamMember.User)
        );

        return usersResponseDto;
    }
}
