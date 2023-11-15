using Shared.Repository.NoSql;

namespace ReportHub.DAL.Models;

public interface IProjectReportInfoRepository : IMongoRepository<ProjectReportInfo>
{
    Task<IList<string>> GetAllUsersReportsPaths(
        int id,
        CancellationToken cancellationToken = default
    );
}
