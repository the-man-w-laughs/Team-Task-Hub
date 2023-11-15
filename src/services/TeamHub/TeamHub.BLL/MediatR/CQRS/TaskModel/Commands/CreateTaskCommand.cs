using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Commands;

public record CreateTaskCommand(int ProjectId, TaskModelRequestDto TaskModelRequestDto)
    : IRequest<int>;
