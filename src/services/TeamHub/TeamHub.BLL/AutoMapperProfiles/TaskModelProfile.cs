using TeamHub.BLL.Dtos;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.AutoMapperProfiles
{
    public class TaskModelProfile : BaseProfile
    {
        public TaskModelProfile()
        {
            CreateMap<TaskModelRequestDto, TaskModel>();
            CreateMap<TaskModel, TaskModelResponseDto>()
                .ForMember(
                    project => project.TasksHandlers,
                    expression =>
                        expression.MapFrom(
                            src =>
                                src.TasksHandlers != null ? GetUsersList(src.TasksHandlers) : null
                        )
                );
        }

        private List<User> GetUsersList(ICollection<TaskHandler> teamMembers)
        {
            List<User> users = new List<User>();
            foreach (var member in teamMembers)
            {
                var user = member.TeamMember.User;
                users.Add(user);
            }

            return users;
        }
    }
}
