using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using Shared.Extensions;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Queries;

public class GetAllUsersProjectsQueryHandler
    : IRequestHandler<GetAllUsersProjectsQuery, IEnumerable<ProjectResponseDto>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public GetAllUsersProjectsQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IProjectRepository projectRepository,
        IMapper mapper
    )
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<ProjectResponseDto>> Handle(
        GetAllUsersProjectsQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor.GetUserId();

        var userProjects = await _projectRepository.GetAllAsync(
            project => project.TeamMembers.Any(tm => tm.UserId == userId)
        );

        var projectResponseDtos = userProjects.Select(
            project => _mapper.Map<ProjectResponseDto>(project)
        );

        return projectResponseDtos;
    }
}
