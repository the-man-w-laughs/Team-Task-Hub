using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Queries;

public record GetAllUsersProjectsQuery(int Offset, int Limit)
    : IRequest<IEnumerable<ProjectResponseDto>> { }
