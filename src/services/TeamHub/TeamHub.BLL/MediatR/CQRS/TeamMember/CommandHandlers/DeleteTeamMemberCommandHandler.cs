using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
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
    private readonly IProjectService _projectService;
    private readonly ITeamMemberService _teamMemberService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public DeleteTeamMemberCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IProjectService projectService,
        ITeamMemberService teamMemberService,
        IUserService userService,
        IMapper mapper
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _projectService = projectService;
        _teamMemberService = teamMemberService;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<TeamMemberResponseDto> Handle(
        DeleteTeamMemberCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        // check if current user exists
        await _userService.GetUserAsync(userId, cancellationToken);

        // get required project
        var project = await _projectService.GetProjectAsync(request.ProjectId, cancellationToken);

        // only team members can have access to this section
        await _teamMemberService.GetTeamMemberAsync(userId, request.ProjectId, cancellationToken);

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

        // remove team member
        var deletedTeamMember = _teamMemberService.RemoveTeamMemberAsync(
            request.UserId,
            request.ProjectId,
            cancellationToken
        );
        var result = _mapper.Map<TeamMemberResponseDto>(deletedTeamMember);

        return result;
    }
}
