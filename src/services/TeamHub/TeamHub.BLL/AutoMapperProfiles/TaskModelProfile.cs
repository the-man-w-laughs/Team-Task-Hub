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
                                src.TasksHandlers != null
                                    ? src.TasksHandlers
                                        .Select(member => member.TeamMember.User)
                                        .ToList()
                                    : null
                        )
                )
                .ForMember(
                    task => task.IsCompleted,
                    expression => expression.MapFrom(src => src.IsCompleted == 1)
                );

            CreateMap<TaskModel, ProjectTaskResponseDto>()
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
            List<int> userIds = new List<int>();
            foreach (var member in teamMembers)
            {
                var id = member.TeamMember.UserId;
                userIds.Add(id);
            }

            return userIds;
        }
    }
}
