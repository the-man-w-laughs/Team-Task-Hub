using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.Extensions;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Queries;

public class GetAllUsersProjectsQueryHandler
    : IRequestHandler<GetAllUsersProjectsQuery, IEnumerable<ProjectResponseDto>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetAllUsersProjectsQueryHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<ProjectResponseDto>> Handle(
        GetAllUsersProjectsQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();
        return new List<ProjectResponseDto>();
    }
}
