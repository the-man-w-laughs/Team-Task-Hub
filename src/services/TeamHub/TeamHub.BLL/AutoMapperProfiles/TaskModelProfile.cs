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
                                src.TasksHandlers != null
                                    ? src.TasksHandlers
                                        .Select(member => member.TeamMember.User)
                                        .ToList()
                                    : null
                        )
                );
        }
    }
}
