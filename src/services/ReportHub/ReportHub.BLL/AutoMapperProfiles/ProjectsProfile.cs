using ReportHub.BLL.Dtos;
using ReportHub.DAL.Models;

namespace ReportHub.BLL.AutoMapperProfiles
{
    public class ProjectsProfile : BaseProfile
    {
        public ProjectsProfile()
        {
            CreateMap<Report, ReportDto>()
                .ForMember(dest => dest.Path, opt => opt.MapFrom(src => src.Path))
                .ForMember(dest => dest.GeneratedAt, opt => opt.MapFrom(src => src.GeneratedAt));
        }
    }
}
