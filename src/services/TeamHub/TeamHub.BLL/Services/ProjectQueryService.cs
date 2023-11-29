using Shared.Exceptions;
using TeamHub.BLL.Contracts;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.Services
{
    public class ProjectQueryService : IProjectQueryService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectQueryService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Project> GetExistingProjectAsync(
            int projectId,
            CancellationToken cancellationToken
        )
        {
            var project = await _projectRepository.GetByIdAsync(projectId, cancellationToken);

            if (project == null)
            {
                throw new NotFoundException($"Project with id {projectId} not found.");
            }

            return project;
        }
    }
}
