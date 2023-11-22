using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using Shared.Extensions;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;
using TeamHub.BLL.Contracts;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Commands;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public CreateProjectCommandHandler(
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

    public async Task<ProjectResponseDto> Handle(
        CreateProjectCommand request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        // check if current user exists
        await _userService.GetUserAsync(userId, cancellationToken);

        // create new project
        var projectToAdd = _mapper.Map<Project>(request.ProjectRequestDto);

        // current user will be author
        projectToAdd.AuthorId = userId;

        // current user will be team member
        projectToAdd.TeamMembers.Add(new TeamMember() { UserId = userId });
        var addedProject = await _projectRepository.AddAsync(projectToAdd, cancellationToken);
        await _projectRepository.SaveAsync(cancellationToken);
        var response = _mapper.Map<ProjectResponseDto>(addedProject);

        return response;
    }
}
