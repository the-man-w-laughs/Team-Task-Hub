using TeamHub.DAL.Models;
using TeamHub.BLL.Dtos.TaskHandler;

namespace TeamHub.BLL.AutoMapperProfiles
{
    public class TaskHandlerProfile : BaseProfile
    {
        public TaskHandlerProfile()
        {
            CreateMap<TaskHandler, TaskHandlerResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.TeamMember.UserId))
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
