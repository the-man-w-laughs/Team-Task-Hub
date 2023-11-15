using MediatR;

namespace TeamHub.BLL.MediatR.CQRS.TaskHandlers.Commands;

public record CreateTaskHandlerCommand(int TaskId, int UserId) : IRequest<int>;
