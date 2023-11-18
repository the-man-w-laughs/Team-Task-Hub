using Shared.gRPC.FullProjectResponse;
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
                            src =>
                                src.TeamMembers != null
                                    ? src.TeamMembers.Select(member => member.User).ToList()
                                    : null
                        )
                );

            CreateMap<Project, FullProjectInfoResponse>()
                .ForMember(
                    project => project.TeamMembers,
                    expression =>
                        expression.MapFrom(
                            src =>
                                src.TeamMembers != null
                                    ? src.TeamMembers.Select(member => member.User).ToList()
                                    : null
                        )
                );
        }
    }
}
