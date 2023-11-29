using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using Shared.Extensions;
using TeamHub.BLL.Contracts;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Queries;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IUserQueryService _userService;
    private readonly IProjectQueryService _projectService;
    private readonly ITeamMemberQueryService _teamMemberService;

    public GetProjectByIdQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IUserQueryService userService,
        IProjectQueryService projectService,
        ITeamMemberQueryService teamMemberService
    )
    {
        _mapper = mapper;
        _userService = userService;
        _projectService = projectService;
        _teamMemberService = teamMemberService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ProjectResponseDto> Handle(
        GetProjectByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

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

        return response;
    }
}
