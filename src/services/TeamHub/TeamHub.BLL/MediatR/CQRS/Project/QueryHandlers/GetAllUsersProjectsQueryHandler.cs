using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using Shared.Extensions;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.BLL.Contracts;
using Microsoft.Extensions.Logging;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Queries;

public class GetAllUsersProjectsQueryHandler
    : IRequestHandler<GetAllUsersProjectsQuery, IEnumerable<ProjectResponseDto>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    private readonly IUserQueryService _userService;
    private readonly ILogger<GetAllUsersProjectsQueryHandler> _logger;

    public GetAllUsersProjectsQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IProjectRepository projectRepository,
        IMapper mapper,
        IUserQueryService userService,
        ILogger<GetAllUsersProjectsQueryHandler> logger
    )
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
        _userService = userService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<ProjectResponseDto>> Handle(
        GetAllUsersProjectsQuery request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();
        _logger.LogInformation("User {UserId} retrieving their projects as team member.", userId);
        // check if current user exists
        await _userService.GetExistingUserAsync(userId, cancellationToken);

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
        _logger.LogInformation("Projects retrieved successfully for user {UserId}.", userId);

        return projectResponseDtos;
    }
}
