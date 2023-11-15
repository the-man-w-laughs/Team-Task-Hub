using ReportHub.BLL.Dtos;
using ReportHub.DAL.Models;

namespace ReportHub.BLL.AutoMapperProfiles
{
    public class ProjectsProfile : BaseProfile
    {
        public ProjectsProfile()
        {
            CreateMap<ProjectReportInfo, ProjectReportInfoDto>();
            CreateMap<Report, ReportDto>();
        }
    }
}
