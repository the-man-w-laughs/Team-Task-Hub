using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Commands;

public record CreateProjectCommand(ProjectRequestDto ProjectRequestDto)
    : IRequest<ProjectResponseDto>;
