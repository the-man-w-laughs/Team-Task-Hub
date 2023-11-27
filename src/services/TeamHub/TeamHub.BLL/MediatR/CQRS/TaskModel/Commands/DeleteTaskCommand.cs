using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Commands;

public record DeleteTaskCommand(int TaskId) : IRequest<TaskModelResponseDto>;
