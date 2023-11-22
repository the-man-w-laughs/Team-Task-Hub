using MediatR;
using TeamHub.BLL.Dtos.TaskHandler;

namespace TeamHub.BLL.MediatR.CQRS.TaskHandlers.Commands;

public record DeleteTaskHandlerCommand(int TaskId, int UserId) : IRequest<TaskHandlerResponseDto>;
