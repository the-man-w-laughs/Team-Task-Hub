using Moq;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.Tests.Helpers
{
    public class ProjectRepositoryHelper
    {
        private readonly Mock<IProjectRepository> _projectRepositoryMock;

        public ProjectRepositoryHelper(Mock<IProjectRepository> projectRepositoryMock)
        {
            _projectRepositoryMock = projectRepositoryMock;
        }

        public void SetupAddAsync(
            Project project,
            CancellationToken cancellationToken,
            Project result
        )
        {
            _projectRepositoryMock
                .Setup(x => x.AddAsync(project, cancellationToken))
                .ReturnsAsync(result);
        }

        public void SetupSaveAsync(CancellationToken cancellationToken)
        {
            _projectRepositoryMock
                .Setup(x => x.SaveAsync(cancellationToken))
                .Returns(Task.CompletedTask);
        }

        public void SetupDelete(Project project)
        {
            _projectRepositoryMock.Setup(x => x.Delete(project));
        }

        public void SetupUpdate(Project project)
        {
            _projectRepositoryMock.Setup(x => x.Update(project));
        }
    }
}
