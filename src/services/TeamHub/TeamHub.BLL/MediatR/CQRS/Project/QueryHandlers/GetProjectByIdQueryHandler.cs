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
    private readonly IUserService _userService;
    private readonly IProjectService _projectService;
    private readonly ITeamMemberService _teamMemberService;

    public GetProjectByIdQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IUserService userService,
        IProjectService projectService,
        ITeamMemberService teamMemberService
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
        await _userService.GetUserAsync(userId, cancellationToken);

        // get current project
        var project = await _projectService.GetProjectAsync(request.ProjectId, cancellationToken);
        var response = _mapper.Map<ProjectResponseDto>(project);

        // only team member has access to project
        var projectId = request.ProjectId;
        await _teamMemberService.GetTeamMemberAsync(userId, projectId, cancellationToken);

        return response;
    }
}
