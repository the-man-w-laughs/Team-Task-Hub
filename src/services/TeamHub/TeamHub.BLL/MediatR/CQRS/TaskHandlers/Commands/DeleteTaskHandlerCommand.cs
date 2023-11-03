using MediatR;

namespace TeamHub.BLL.MediatR.CQRS.TaskHandlers.Commands;

public record DeleteTaskHandlerCommand(int TaskId, int UserId) : IRequest<int>;
