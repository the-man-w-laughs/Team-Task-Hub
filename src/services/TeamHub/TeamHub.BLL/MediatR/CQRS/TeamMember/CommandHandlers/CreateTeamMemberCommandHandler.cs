using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Dtos.TeamMember;

namespace TeamHub.BLL.MediatR.CQRS.TeamMembers.Commands;

public class CreateTeamMemberCommandHandler
    : IRequestHandler<CreateTeamMemberCommand, TeamMemberResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITeamMemberService _teamMemberService;
    private readonly IProjectService _projectService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public CreateTeamMemberCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        ITeamMemberService teamMemberService,
        IProjectService projectService,
        IUserService userService,
        IMapper mapper
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _teamMemberService = teamMemberService;
        _projectService = projectService;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<TeamMemberResponseDto> Handle(
        CreateTeamMemberCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        // check if current user exists
        await _userService.GetUserAsync(userId, cancellationToken);

        // check if requested project exists
        await _projectService.GetProjectAsync(request.ProjectId, cancellationToken);

        // only team members can add other users to project
        await _teamMemberService.GetTeamMemberAsync(userId, request.ProjectId, cancellationToken);

        // create new team member
        var addedTeamMember = await _teamMemberService.AddTeamMemberAsync(
            request.UserId,
            request.ProjectId,
            cancellationToken
        );
        var result = _mapper.Map<TeamMemberResponseDto>(addedTeamMember);

        return result;
    }
}
