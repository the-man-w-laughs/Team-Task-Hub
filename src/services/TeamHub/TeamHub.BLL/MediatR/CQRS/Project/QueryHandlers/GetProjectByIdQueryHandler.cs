using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using Shared.Extensions;
using TeamHub.BLL.Contracts;
using Microsoft.Extensions.Logging;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Queries;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IUserQueryService _userService;
    private readonly IProjectQueryService _projectService;
    private readonly ITeamMemberQueryService _teamMemberService;
    private readonly ILogger<GetProjectByIdQueryHandler> _logger;

    public GetProjectByIdQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IUserQueryService userService,
        IProjectQueryService projectService,
        ITeamMemberQueryService teamMemberService,
        ILogger<GetProjectByIdQueryHandler> logger
    )
    {
        _mapper = mapper;
        _userService = userService;
        _projectService = projectService;
        _teamMemberService = teamMemberService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ProjectResponseDto> Handle(
        GetProjectByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        _logger.LogInformation(
            "User {UserId} retrieving project with ID {ProjectId}.",
            userId,
            request.ProjectId
        );

        // check if current user exists
        await _userService.GetExistingUserAsync(userId, cancellationToken);

        // get current project
        var project = await _projectService.GetExistingProjectAsync(
            request.ProjectId,
            cancellationToken
        );
        var response = _mapper.Map<ProjectResponseDto>(project);

        // only team member has access to project
        var projectId = request.ProjectId;
        await _teamMemberService.GetExistingTeamMemberAsync(userId, projectId, cancellationToken);
        _logger.LogInformation(
            "Project with ID {ProjectId} retrieved successfully by user {UserId}.",
            request.ProjectId,
            userId
        );

        return response;
    }
}
