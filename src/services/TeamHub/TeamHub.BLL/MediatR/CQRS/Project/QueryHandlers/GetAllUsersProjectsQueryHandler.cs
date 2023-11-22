using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using Shared.Extensions;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.BLL.Contracts;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Queries;

public class GetAllUsersProjectsQueryHandler
    : IRequestHandler<GetAllUsersProjectsQuery, IEnumerable<ProjectResponseDto>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public GetAllUsersProjectsQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IProjectRepository projectRepository,
        IMapper mapper,
        IUserService userService
    )
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<ProjectResponseDto>> Handle(
        GetAllUsersProjectsQuery request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        // check if current user exists
        await _userService.GetUserAsync(userId, cancellationToken);

        // get all projects where user is team member
        var userProjects = await _projectRepository.GetAllAsync(
            project => project.TeamMembers.Any(tm => tm.UserId == userId),
            request.Offset,
            request.Limit,
            cancellationToken
        );
        var projectResponseDtos = userProjects.Select(
            project => _mapper.Map<ProjectResponseDto>(project)
        );

        return projectResponseDtos;
    }
}
