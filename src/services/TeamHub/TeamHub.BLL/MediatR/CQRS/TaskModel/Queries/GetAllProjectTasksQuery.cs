using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Queries;

public record GetAllProjectsTasksQuery(int ProjectId, int Offset, int Limit)
    : IRequest<IEnumerable<TaskModelResponseDto>>;
