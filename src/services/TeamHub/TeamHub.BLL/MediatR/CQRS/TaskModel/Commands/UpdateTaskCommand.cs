using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Commands;

public record UpdateTaskCommand(int TaskId, TaskModelRequestDto TaskModelRequestDto)
    : IRequest<int>;
