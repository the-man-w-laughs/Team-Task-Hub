using MassTransit;
using ReportHub.DAL.Contracts;
using ReportHub.DAL.Models;
using Shared.SharedModels;

namespace ReportHub.BLL.MassTransit.Consumers
{
    public class UserDeletedMessageConsumer : IConsumer<UserDeletedMessage>
    {
        private readonly IMinioRepository _fileRepository;
        private readonly IProjectReportInfoRepository _projectReportInfoRepository;

        public UserDeletedMessageConsumer(
            IMinioRepository fileRepository,
            IProjectReportInfoRepository projectReportInfoRepository
        )
        {
            _projectReportInfoRepository = projectReportInfoRepository;
            _fileRepository = fileRepository;
        }

        public async Task Consume(ConsumeContext<UserDeletedMessage> context)
        {
            var filesToDelete = await _projectReportInfoRepository.GetAllUsersReportsPaths(
                context.Message.UserId
            );

            await _fileRepository.DeleteFilesFromMinioAsync(filesToDelete);
        }
    }
}
