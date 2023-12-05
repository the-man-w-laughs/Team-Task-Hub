using Moq;
using TeamHub.BLL.Contracts;
using TeamHub.DAL.Models;

namespace TeamHub.Tests.Helpers
{
    public class TaskQueryServiceHelper
    {
        private readonly Mock<ITaskQueryService> _taskQueryServiceMock;

        public TaskQueryServiceHelper(Mock<ITaskQueryService> taskQueryServiceMock)
        {
            _taskQueryServiceMock = taskQueryServiceMock;
        }

        public void SetupGetExistingTaskAsync(
            int taskId,
            CancellationToken cancellationToken,
            TaskModel result
        )
        {
            _taskQueryServiceMock
                .Setup(x => x.GetExistingTaskAsync(taskId, cancellationToken))
                .ReturnsAsync(result);
        }

        public void SetupGetExistingTaskAsync(
            int taskId,
            CancellationToken cancellationToken,
            Exception exceptionToThrow
        )
        {
            _taskQueryServiceMock
                .Setup(x => x.GetExistingTaskAsync(taskId, cancellationToken))
                .ThrowsAsync(exceptionToThrow);
        }
    }
}
