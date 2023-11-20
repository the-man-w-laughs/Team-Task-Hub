using Shared.gRPC.FullProjectResponse;
using TeamHub.BLL.Dtos;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.AutoMapperProfiles
{
    public class TaskModelsProfile : BaseProfile
    {
        public TaskModelsProfile()
        {
            CreateMap<TaskModelRequestDto, TaskModel>()
                .ForMember(
                    task => task.PriorityId,
                    expression => expression.MapFrom(src => src.PriorityId)
                )
                .ForMember(
                    task => task.Content,
                    expression => expression.MapFrom(src => src.Content)
                )
                .ForMember(
                    task => task.Deadline,
                    expression => expression.MapFrom(src => src.Deadline)
                );

            CreateMap<TaskModel, TaskModelResponseDto>()
                .ForMember(task => task.Id, expression => expression.MapFrom(src => src.Id))
                .ForMember(
                    task => task.CreatorId,
                    expression => expression.MapFrom(src => src.TeamMember.UserId)
                )
                .ForMember(
                    task => task.ProjectsId,
                    expression => expression.MapFrom(src => src.ProjectId)
                )
                .ForMember(
                    task => task.PriorityId,
                    expression => expression.MapFrom(src => src.PriorityId)
                )
                .ForMember(
                    task => task.Content,
                    expression => expression.MapFrom(src => src.Content)
                )
                .ForMember(
                    task => task.Deadline,
                    expression => expression.MapFrom(src => src.Deadline)
                )
                .ForMember(
                    task => task.IsCompleted,
                    expression => expression.MapFrom(src => src.IsCompleted == 1)
                )
                .ForMember(
                    task => task.CreatedAt,
                    expression => expression.MapFrom(src => src.CreatedAt)
                )
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
                );

            CreateMap<TaskModel, ProjectTaskDataContract>()
                .ForMember(task => task.Id, expression => expression.MapFrom(src => src.Id))
                .ForMember(
                    task => task.CreatorId,
                    expression => expression.MapFrom(src => src.TeamMember.UserId)
                )
                .ForMember(
                    task => task.PriorityId,
                    expression => expression.MapFrom(src => src.PriorityId)
                )
                .ForMember(
                    task => task.Content,
                    expression => expression.MapFrom(src => src.Content)
                )
                .ForMember(
                    task => task.Deadline,
                    expression => expression.MapFrom(src => src.Deadline)
                )
                .ForMember(
                    task => task.IsCompleted,
                    expression => expression.MapFrom(src => src.IsCompleted == 1)
                )
                .ForMember(
                    task => task.CreatedAt,
                    expression => expression.MapFrom(src => src.CreatedAt)
                )
                .ForMember(
                    task => task.TasksHandlersIds,
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

            CreateMap<TaskModel, ShortTaskModelResponseDto>()
                .ForMember(task => task.Id, expression => expression.MapFrom(src => src.Id))
                .ForMember(
                    task => task.PriorityId,
                    expression => expression.MapFrom(src => src.PriorityId)
                )
                .ForMember(
                    task => task.Content,
                    expression => expression.MapFrom(src => src.Content)
                )
                .ForMember(
                    task => task.Deadline,
                    expression => expression.MapFrom(src => src.Deadline)
                )
                .ForMember(
                    task => task.CreatedAt,
                    expression => expression.MapFrom(src => src.CreatedAt)
                );
        }
    }
}
