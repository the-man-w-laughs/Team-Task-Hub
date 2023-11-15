using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using TeamHub.DAL.Contracts.Repositories;
using Shared.Exceptions;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Commands;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, int>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectRepository _projectRepository;

    public DeleteProjectCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IProjectRepository projectRepository
    )
    {
        _projectRepository = projectRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.GetUserId();

        var project = await _projectRepository.GetProjectByIdAsync(request.ProjectId);

        if (userId != project.AuthorId)
        {
            throw new ForbiddenException(
                $"User with id {userId} doesn't have rights to alter project with id {project.Id}."
            );
        }

        _projectRepository.Delete(project);
        await _projectRepository.SaveAsync();

        return project.Id;
    }
}
