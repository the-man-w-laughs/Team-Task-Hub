using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using Shared.Extensions;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Dtos;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Commands;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    private readonly IUserQueryService _userService;
    private readonly IProjectQueryService _projectService;

    public UpdateProjectCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IProjectRepository projectRepository,
        IMapper mapper,
        IUserQueryService userService,
        IProjectQueryService projectService
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _projectRepository = projectRepository;
        _mapper = mapper;
        _userService = userService;
        _projectService = projectService;
    }

    public async Task<ProjectResponseDto> Handle(
        UpdateProjectCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        // check if current user exists
        await _userService.GetExistingUserAsync(userId, cancellationToken);

        // get target project
        var project = await _projectService.GetExistingProjectAsync(
            request.ProjectId,
            cancellationToken
        );

        // author can update project
        if (userId != project.AuthorId)
        {
            throw new ForbiddenException(
                $"User with id {userId} doesn't have rights to alter project with id {project.Id}."
            );
        }

        // update project
        _mapper.Map(request.ProjectRequestDto, project);
        _projectRepository.Update(project);
        await _projectRepository.SaveAsync(cancellationToken);
        var result = _mapper.Map<ProjectResponseDto>(project);

        return result;
    }
}
