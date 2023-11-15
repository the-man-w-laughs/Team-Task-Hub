using MediatR;
using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Queries;

public record GetFullProjectByIdQuery(int ProjectId) : IRequest<FullProjectResponseDto>;
