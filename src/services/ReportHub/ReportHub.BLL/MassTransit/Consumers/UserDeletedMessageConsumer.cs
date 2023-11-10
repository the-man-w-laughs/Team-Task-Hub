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
            var usersProjects = await _projectReportInfoRepository.GetAllAsync(
                project => project.ProjectAuthorId == context.Message.Id
            );

            foreach (var project in usersProjects)
            {
                foreach (var report in project.Reports)
                {
                    await _fileRepository.DeleteFileFromMinioAsync(report.Path);
                }
            }
        }
    }
}
