using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.TaskHandlers.Queries;

public record GetAllTaskHandlersQuery(int TaskId, int Offset, int Limit)
    : IRequest<IEnumerable<UserResponseDto>>;
