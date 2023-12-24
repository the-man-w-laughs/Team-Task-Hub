using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using TeamHub.DAL.Contracts.Repositories;
using Shared.Exceptions;
using TeamHub.BLL.Dtos;
using AutoMapper;
using TeamHub.BLL.Contracts;
using Microsoft.Extensions.Logging;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Commands;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, ProjectResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    private readonly IUserQueryService _userService;
    private readonly IProjectQueryService _projectService;
    private readonly ILogger<DeleteProjectCommandHandler> _logger;

    public DeleteProjectCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IProjectRepository projectRepository,
        IMapper mapper,
        IUserQueryService userService,
        IProjectQueryService projectService,
        ILogger<DeleteProjectCommandHandler> logger
    )
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
        _userService = userService;
        _projectService = projectService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ProjectResponseDto> Handle(
        DeleteProjectCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        _logger.LogInformation(
            "User {UserId} is attempting to delete project {ProjectId}.",
            userId,
            request.ProjectId
        );

        // check if current user exists
        await _userService.GetExistingUserAsync(userId, cancellationToken);

        // retrieve target project
        var project = await _projectService.GetExistingProjectAsync(
            request.ProjectId,
            cancellationToken
        );

        // only author can delete project
        if (userId != project.AuthorId)
        {
            throw new ForbiddenException(
                $"User with id {userId} doesn't have rights to delete project with id {project.Id}."
            );
        }

        // delete project
        var result = _mapper.Map<ProjectResponseDto>(project);
        _projectRepository.Delete(project);
        await _projectRepository.SaveAsync(cancellationToken);

        _logger.LogInformation("Project {ProjectId} deleted successfully.", result.Id);

        return result;
    }
}
