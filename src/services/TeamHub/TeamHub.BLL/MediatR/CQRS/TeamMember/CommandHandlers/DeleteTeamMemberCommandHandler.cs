using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Shared.Extensions;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Dtos.TeamMember;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.TeamMembers.Commands;

public class DeleteTeamMemberCommandHandler
    : IRequestHandler<DeleteTeamMemberCommand, TeamMemberResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectQueryService _projectService;
    private readonly ITeamMemberQueryService _teamMemberService;
    private readonly IUserQueryService _userService;
    private readonly IMapper _mapper;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly ILogger<DeleteTeamMemberCommandHandler> _logger;

    public DeleteTeamMemberCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IProjectQueryService projectService,
        ITeamMemberQueryService teamMemberService,
        IUserQueryService userService,
        IMapper mapper,
        ITeamMemberRepository teamMemberRepository,
        ILogger<DeleteTeamMemberCommandHandler> logger
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _projectService = projectService;
        _teamMemberService = teamMemberService;
        _userService = userService;
        _mapper = mapper;
        _teamMemberRepository = teamMemberRepository;
        _logger = logger;
    }

    public async Task<TeamMemberResponseDto> Handle(
        DeleteTeamMemberCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        _logger.LogInformation(
            "User {UserId} is trying to delete user with id {TargetUserId} from project with id {ProjectId}.",
            userId,
            request.UserId,
            request.ProjectId
        );

        // check if current user exists
        await _userService.GetExistingUserAsync(userId, cancellationToken);

        // get required project
        var project = await _projectService.GetExistingProjectAsync(
            request.ProjectId,
            cancellationToken
        );

        // only team members can have access to this section
        await _teamMemberService.GetExistingTeamMemberAsync(
            userId,
            request.ProjectId,
            cancellationToken
        );

        // no one except project author cannot delete other team members
        if (project.AuthorId != userId && userId != request.UserId)
        {
            throw new ForbiddenException(
                $"User with id {request.UserId} cannot delete user with id {request.ProjectId}."
            );
        }

        // author cannot delete themselves
        if (project.AuthorId == userId && userId == request.UserId)
        {
            throw new ForbiddenException(
                $"User with id {request.UserId} cannot delete themselves from project with id {request.ProjectId}, because user is the author of the project. Consider deletion of project."
            );
        }

        // check if target team member exists
        var teamMemberToDelete = await _teamMemberService.GetExistingTeamMemberAsync(
            request.UserId,
            request.ProjectId,
            cancellationToken
        );

        // remove team member
        _teamMemberRepository.Delete(teamMemberToDelete);
        await _teamMemberRepository.SaveAsync(cancellationToken);

        var result = _mapper.Map<TeamMemberResponseDto>(teamMemberToDelete);

        _logger.LogInformation(
            "Successfully removed user {UserId} from project with id {ProjectId}.",
            request.UserId,
            request.ProjectId
        );

        return result;
    }
}
