using Shared.gRPC.FullProjectResponse;
using TeamHub.BLL.Dtos;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.AutoMapperProfiles
{
    public class ProjectsProfile : BaseProfile
    {
        public ProjectsProfile()
        {
            CreateMap<ProjectRequestDto, Project>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<Project, ProjectResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Creator))
                .ForMember(
                    project => project.TeamMembers,
                    opt =>
                        opt.MapFrom(
                            src =>
                                src.TeamMembers != null
                                    ? src.TeamMembers.Select(member => member.User).ToList()
                                    : null
                        )
                );

            CreateMap<Project, FullProjectInfoResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Creator))
                .ForMember(dest => dest.TeamMembers, opt => opt.MapFrom(src => src.TeamMembers))
                .ForMember(
                    project => project.TeamMembers,
                    opt =>
                        opt.MapFrom(
                            src =>
                                src.TeamMembers != null
                                    ? src.TeamMembers.Select(member => member.User).ToList()
                                    : null
                        )
                );
        }
    }
}
