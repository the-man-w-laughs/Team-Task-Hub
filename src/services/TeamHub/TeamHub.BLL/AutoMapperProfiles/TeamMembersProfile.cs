using TeamHub.DAL.Models;
using TeamHub.BLL.Dtos.TeamMember;

namespace TeamHub.BLL.AutoMapperProfiles
{
    public class TeamMembersProfile : BaseProfile
    {
        public TeamMembersProfile()
        {
            CreateMap<TeamMember, TeamMemberResponseDto>()
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Projects.Name))
                .ForMember(
                    dest => dest.Tasks,
                    opt =>
                        opt.MapFrom(
                            src => src.TasksHandlers.Select(taskHandler => taskHandler.Tasks)
                        )
                );
        }
    }
}
