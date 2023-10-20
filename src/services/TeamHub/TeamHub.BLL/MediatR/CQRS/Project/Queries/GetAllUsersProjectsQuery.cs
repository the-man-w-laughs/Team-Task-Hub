using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Queries;

public record GetAllUsersProjectsQuery() : IRequest<IEnumerable<ProjectResponseDto>>;
