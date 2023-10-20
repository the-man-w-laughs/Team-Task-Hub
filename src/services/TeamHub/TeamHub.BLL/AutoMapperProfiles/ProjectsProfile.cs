using TeamHub.BLL.Dtos;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.AutoMapperProfiles
{
    public class ProjectsProfile : BaseProfile
    {
        public ProjectsProfile()
        {
            CreateMap<ProjectRequestDto, Project>();
            CreateMap<Project, ProjectResponseDto>()
                .ForMember(
                    project => project.TeamMembers,
                    expression =>
                        expression.MapFrom(
                            src => src.TeamMembers != null ? GetUsersList(src.TeamMembers) : null
                        )
                );
        }

        private List<User> GetUsersList(ICollection<TeamMember> teamMembers)
        {
            List<User> users = new List<User>();
            foreach (var member in teamMembers)
            {
                var user = member.User;
                users.Add(user);
            }
            return users;
        }
    }
}
