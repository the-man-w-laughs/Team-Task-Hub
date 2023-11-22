using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using Shared.Extensions;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.BLL.Contracts;

namespace TeamHub.BLL.MediatR.CQRS.TeamMembers.Queries;

public class GetAllProjectsTeamMembersQueryHandler
    : IRequestHandler<GetAllProjectsTeamMembersQuery, IEnumerable<UserResponseDto>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly IMapper _mapper;
    private readonly IProjectService _projectService;
    private readonly ITeamMemberService _teamMemberService;
    private readonly IUserService _userService;

    public GetAllProjectsTeamMembersQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        ITeamMemberRepository teamMemberRepository,
        IMapper mapper,
        IProjectService projectService,
        ITeamMemberService teamMemberService,
        IUserService userService
    )
    {
        _teamMemberRepository = teamMemberRepository;
        _mapper = mapper;
        _projectService = projectService;
        _teamMemberService = teamMemberService;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<UserResponseDto>> Handle(
        GetAllProjectsTeamMembersQuery request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        // check if current user exists
        await _userService.GetUserAsync(userId, cancellationToken);

        // get requested project
        var project = await _projectService.GetProjectAsync(request.ProjectId, cancellationToken);

        // only team members have access to related projects
        var teamMember = await _teamMemberService.GetTeamMemberAsync(
            userId,
            request.ProjectId,
            cancellationToken
        );

        // get all project's team members
        var teamMembers = await _teamMemberRepository.GetAllAsync(
            teamMember => teamMember.ProjectId == request.ProjectId,
            request.Offset,
            request.Limit,
            cancellationToken
        );
        var usersResponseDto = teamMembers.Select(
            teamMember => _mapper.Map<UserResponseDto>(teamMember.User)
        );

        return usersResponseDto;
    }
}
