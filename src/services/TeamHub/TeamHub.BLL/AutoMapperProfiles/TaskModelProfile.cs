using Shared.gRPC.FullProjectResponse;
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
                    task => task.TasksHandlers,
                    expression =>
                        expression.MapFrom(
                            src =>
                                src.TasksHandlers != null ? GetUsersList(src.TasksHandlers) : null
                        )
                )
                .ForMember(
                    task => task.IsCompleted,
                    expression => expression.MapFrom(src => src.IsCompleted == 1)
                );

            CreateMap<TaskModel, ProjectTaskDataContract>()
                .ForMember(
                    task => task.TasksHandlersIds,
                    expression =>
                        expression.MapFrom(
                            src =>
                                src.TasksHandlers != null ? GetUserIdList(src.TasksHandlers) : null
                        )
                )
                .ForMember(
                    task => task.IsCompleted,
                    expression => expression.MapFrom(src => src.IsCompleted == 1)
                );
        }

        private List<int> GetUserIdList(ICollection<TaskHandler> teamMembers)
        {
            var userIds = new List<int>();
            foreach (var member in teamMembers)
            {
                var id = member.TeamMember.UserId;
                userIds.Add(id);
            }

            return userIds;
        }

        private List<User> GetUsersList(ICollection<TaskHandler> teamMembers)
        {
            var users = new List<User>();
            foreach (var member in teamMembers)
            {
                var user = member.TeamMember.User;
                users.Add(user);
            }

            return users;
        }
    }
}
