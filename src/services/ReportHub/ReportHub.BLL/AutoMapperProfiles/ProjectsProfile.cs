using ReportHub.BLL.Dtos;
using ReportHub.BLL.Services;
using ReportHub.DAL.Models;

namespace ReportHub.BLL.AutoMapperProfiles
{
    public class ProjectsProfile : BaseProfile
    {
        public ProjectsProfile()
        {
            CreateMap<ProjectReportInfo, ProjectReportInfoDto>()
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(
                    dest => dest.ProjectAuthorId,
                    opt => opt.MapFrom(src => src.ProjectAuthorId)
                )
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Reports, opt => opt.MapFrom(src => src.Reports));
        }
    }
}
