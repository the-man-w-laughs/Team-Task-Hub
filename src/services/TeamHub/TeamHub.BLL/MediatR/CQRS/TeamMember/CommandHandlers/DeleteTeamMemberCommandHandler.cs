using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using Shared.Extensions;
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
        var userId = _httpContextAccessor.GetUserId();

        var project = await _projectRepository.GetProjectByIdAsync(
            request.ProjectId,
            cancellationToken
        );
        await _teamMemberRepository.GetTeamMemberAsync(
            userId,
            request.ProjectId,
            cancellationToken
        );

        var teamMemberToDelete = await _teamMemberRepository.GetTeamMemberAsync(
            request.UserId,
            request.ProjectId,
            cancellationToken
        );

        if (teamMemberToDelete == null)
        {
            throw new WrongActionException(
                $"User with id {request.UserId} is NOT a part of project with id {request.ProjectId}."
            );
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

        _teamMemberRepository.Delete(teamMemberToDelete);
        await _teamMemberRepository.SaveAsync(cancellationToken);

        return teamMemberToDelete.Id;
    }
}
