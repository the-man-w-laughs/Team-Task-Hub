using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using TeamHub.BLL.Extensions;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.TeamMembers.Commands;

public class DeleteTeamMemberCommandHandler : IRequestHandler<DeleteTeamMemberCommand, int>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly IProjectRepository _projectRepository;

    public DeleteTeamMemberCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        ITeamMemberRepository teamMemberRepository,
        IProjectRepository projectRepository
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _teamMemberRepository = teamMemberRepository;
        _projectRepository = projectRepository;
    }

    public async Task<int> Handle(
        DeleteTeamMemberCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();

        var project = await _projectRepository.GetByIdAsync(request.ProjectId);

        if (project == null)
        {
            throw new NotFoundException($"Project with id {request.ProjectId} was not found.");
        }

        if (project.AuthorId != userId && userId != request.UserId)
        {
            throw new ForbiddenException(
                $"User with id {request.UserId} cannot delete user with id {request.ProjectId}."
            );
        }

        if (project.AuthorId == userId && userId == request.UserId)
        {
            throw new ForbiddenException(
                $"User with id {request.UserId} cannot delete themselves from project with id {request.ProjectId}, because user is the author of the project. Consider deletion of project."
            );
        }

        var teamMemberToDelete = await _teamMemberRepository.GetAsync(
            teamMember =>
                teamMember.UserId == request.UserId && teamMember.ProjectId == request.ProjectId
        );

        if (teamMemberToDelete == null)
        {
            throw new ForbiddenException(
                $"User with id {request.UserId} is not a team member of project with id {request.ProjectId}."
            );
        }
        _teamMemberRepository.Delete(teamMemberToDelete);
        await _teamMemberRepository.SaveAsync();

        return teamMemberToDelete.Id;
    }
}
