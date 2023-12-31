using Moq;
using TeamHub.BLL.Contracts;
using TeamHub.DAL.Models;

namespace TeamHub.Tests.Helpers
{
    public class TeamMemberQueryServiceHelper
    {
        private readonly Mock<ITeamMemberQueryService> _teamMemberQueryServiceMock;

        public TeamMemberQueryServiceHelper(
            Mock<ITeamMemberQueryService> teamMemberQueryServiceMock
        )
        {
            _teamMemberQueryServiceMock = teamMemberQueryServiceMock;
        }

        public void SetupGetExistingTeamMemberAsync(int userId, int projectId, TeamMember result)
        {
            _teamMemberQueryServiceMock
                .Setup(x => x.GetExistingTeamMemberAsync(userId, projectId, CancellationToken.None))
                .ReturnsAsync(result);
        }

        public void SetupGetExistingTeamMemberAsync(
            int userId,
            int projectId,
            Exception exceptionToThrow
        )
        {
            _teamMemberQueryServiceMock
                .Setup(x => x.GetExistingTeamMemberAsync(userId, projectId, CancellationToken.None))
                .Throws(exceptionToThrow);
        }
    }
}
