using ReportHub.BLL.Dtos;
using ReportHub.DAL.Models;

namespace ReportHub.BLL.AutoMapperProfiles
{
    public class ProjectsProfile : BaseProfile
    {
        public ProjectsProfile()
        {
            CreateMap<Report, ReportDto>()
                .ForMember(dest => dest.Path, expression => expression.MapFrom(src => src.Path))
                .ForMember(
                    dest => dest.GeneratedAt,
                    expression => expression.MapFrom(src => src.GeneratedAt)
                );
        }
    }
}
