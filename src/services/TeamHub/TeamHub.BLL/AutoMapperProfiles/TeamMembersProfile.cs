using TeamHub.DAL.Models;
using TeamHub.BLL.Dtos.TeamMember;

namespace TeamHub.BLL.AutoMapperProfiles
{
    public class TeamMembersProfile : BaseProfile
    {
        public TeamMembersProfile()
        {
            CreateMap<TeamMember, TeamMemberDto>()
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
                .ForMember(
                    dest => dest.Tasks,
                    opt =>
                        opt.MapFrom(
                            src => src.TasksHandlers.Select(taskHandler => taskHandler.Task)
                        )
                );

            CreateMap<TeamMember, TeamMemberResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
