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

        public void SetupGetExistingTaskAsync(int taskId, TaskModel result)
        {
            _taskQueryServiceMock
                .Setup(x => x.GetExistingTaskAsync(taskId, CancellationToken.None))
                .ReturnsAsync(result);
        }

        public void SetupGetExistingTaskAsync(int taskId, Exception exceptionToThrow)
        {
            _taskQueryServiceMock
                .Setup(x => x.GetExistingTaskAsync(taskId, CancellationToken.None))
                .ThrowsAsync(exceptionToThrow);
        }
    }
}
