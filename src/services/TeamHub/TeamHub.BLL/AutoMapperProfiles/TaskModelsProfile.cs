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
                .ForMember(task => task.PriorityId, opt => opt.MapFrom(src => src.PriorityId))
                .ForMember(task => task.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(task => task.Deadline, opt => opt.MapFrom(src => src.Deadline))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<TaskModel, TaskModelResponseDto>()
                .ForMember(task => task.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    task => task.CreatorId,
                    opt => opt.MapFrom(src => src.AuthorTeamMember.UserId)
                )
                .ForMember(task => task.ProjectId, opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(task => task.PriorityId, opt => opt.MapFrom(src => src.PriorityId))
                .ForMember(task => task.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(task => task.Deadline, opt => opt.MapFrom(src => src.Deadline))
                .ForMember(
                    task => task.IsCompleted,
                    opt => opt.MapFrom(src => src.IsCompleted == 1)
                )
                .ForMember(task => task.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(
                    task => task.TasksHandlers,
                    opt =>
                        opt.MapFrom(
                            src =>
                                src.TasksHandlers != null
                                    ? src.TasksHandlers
                                        .Select(member => member.TeamMember.User)
                                        .ToList()
                                    : null
                        )
                );

            CreateMap<TaskModel, ProjectTaskDataContract>()
                .ForMember(task => task.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    task => task.CreatorId,
                    opt => opt.MapFrom(src => src.AuthorTeamMember.UserId)
                )
                .ForMember(task => task.PriorityId, opt => opt.MapFrom(src => src.PriorityId))
                .ForMember(task => task.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(task => task.Deadline, opt => opt.MapFrom(src => src.Deadline))
                .ForMember(
                    task => task.IsCompleted,
                    opt => opt.MapFrom(src => src.IsCompleted == 1)
                )
                .ForMember(task => task.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(
                    task => task.TasksHandlersIds,
                    opt =>
                        opt.MapFrom(
                            src =>
                                src.TasksHandlers != null
                                    ? src.TasksHandlers
                                        .Select(member => member.TeamMember.User)
                                        .ToList()
                                    : null
                        )
                );

            CreateMap<TaskModel, ShortTaskModelResponseDto>()
                .ForMember(task => task.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(task => task.PriorityId, opt => opt.MapFrom(src => src.PriorityId))
                .ForMember(task => task.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(task => task.Deadline, opt => opt.MapFrom(src => src.Deadline))
                .ForMember(task => task.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
