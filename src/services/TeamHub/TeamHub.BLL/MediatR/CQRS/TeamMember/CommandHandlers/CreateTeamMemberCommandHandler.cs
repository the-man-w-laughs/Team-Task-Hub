using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using Shared.Extensions;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Dtos.TeamMember;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.MediatR.CQRS.TeamMembers.Commands;

public class CreateTeamMemberCommandHandler
    : IRequestHandler<CreateTeamMemberCommand, TeamMemberResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITeamMemberQueryService _teamMemberService;
    private readonly IProjectQueryService _projectService;
    private readonly IUserQueryService _userService;
    private readonly IMapper _mapper;
    private readonly ITeamMemberRepository _teamMemberRepository;

    public CreateTeamMemberCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        ITeamMemberQueryService teamMemberService,
        IProjectQueryService projectService,
        IUserQueryService userService,
        IMapper mapper,
        ITeamMemberRepository teamMemberRepository
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _teamMemberService = teamMemberService;
        _projectService = projectService;
        _userService = userService;
        _mapper = mapper;
        _teamMemberRepository = teamMemberRepository;
    }

    public async Task<TeamMemberResponseDto> Handle(
        CreateTeamMemberCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var currentUserId = _httpContextAccessor.GetUserId();

        // check if current user exists
        await _userService.GetExistingUserAsync(currentUserId, cancellationToken);

        // check if requested project exists
        await _projectService.GetExistingProjectAsync(request.ProjectId, cancellationToken);

        // only team members can add other users to project
        await _teamMemberService.GetExistingTeamMemberAsync(
            currentUserId,
            request.ProjectId,
            cancellationToken
        );

        // check if target user is already a team member
        var teamMember = await _teamMemberRepository.GetTeamMemberAsync(
            request.UserId,
            request.ProjectId,
            cancellationToken
        );

        if (teamMember != null)
        {
            throw new ForbiddenException(
                $"User with id {request.UserId} is already a team member of project with id {request.ProjectId}."
            );
        }

        // create new team member
        var teamMemberToAdd = new TeamMember()
        {
            ProjectId = request.ProjectId,
            UserId = request.UserId,
            CreatedAt = DateTime.Now
        };
        var addedTeamMember = await _teamMemberRepository.AddAsync(
            teamMemberToAdd,
            cancellationToken
        );
        await _teamMemberRepository.SaveAsync(cancellationToken);
        var result = _mapper.Map<TeamMemberResponseDto>(addedTeamMember);

        return result;
    }
}
