using MediatR;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Commands;

public record DeleteProjectCommand(int ProjectId) : IRequest<int>;
