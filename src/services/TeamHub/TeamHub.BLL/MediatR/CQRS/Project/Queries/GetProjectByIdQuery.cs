using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Queries;

public record GetProjectByIdQuery(int ProjectId) : IRequest<ProjectResponseDto>;
