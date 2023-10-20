using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Queries;

public record GetAllProjectsTasksQuery(int ProjectId) : IRequest<IEnumerable<TaskModelResponseDto>>;
