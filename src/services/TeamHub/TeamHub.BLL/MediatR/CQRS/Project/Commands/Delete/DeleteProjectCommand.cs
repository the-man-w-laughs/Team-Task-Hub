using MediatR;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Commands;

public record DeleteProjectCommand(int id) : IRequest<int?>;
