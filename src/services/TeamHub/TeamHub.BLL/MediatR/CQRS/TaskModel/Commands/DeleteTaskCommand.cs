using MediatR;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Commands;

public record DeleteTaskCommand(int TaskId) : IRequest<int>;
