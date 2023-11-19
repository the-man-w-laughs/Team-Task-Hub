using TeamHub.DAL.Models;
using TeamHub.BLL.Dtos.TeamMember;

namespace TeamHub.BLL.AutoMapperProfiles
{
    public class TeamMemberProfile : BaseProfile
    {
        public TeamMemberProfile()
        {
            CreateMap<TeamMember, TeamMemberResponseDto>()
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Projects.Name))
                .ForMember(
                    dest => dest.Tasks,
                    opt => opt.MapFrom(src => src.TasksHandlers.Select(th => th.Tasks))
                );
        }
    }
}
