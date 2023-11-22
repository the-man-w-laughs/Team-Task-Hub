using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Extensions;
using TeamHub.DAL.Contracts.Repositories;
using Shared.Exceptions;
using TeamHub.BLL.Dtos;
using AutoMapper;
using TeamHub.BLL.Contracts;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Commands;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, ProjectResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IProjectService _projectService;

    public DeleteProjectCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IProjectRepository projectRepository,
        IMapper mapper,
        IUserService userService,
        IProjectService projectService
    )
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
        _userService = userService;
        _projectService = projectService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ProjectResponseDto> Handle(
        DeleteProjectCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        // check if current user exists
        await _userService.GetUserAsync(userId, cancellationToken);

        // retrieve target project
        var project = await _projectService.GetProjectAsync(request.ProjectId, cancellationToken);

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

        return result;
    }
}
