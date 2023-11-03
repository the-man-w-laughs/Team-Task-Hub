using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.Extensions;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Commands;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public CreateProjectCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IProjectRepository projectRepository,
        IMapper mapper
    )
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.GetUserId();

        var projectToAdd = _mapper.Map<Project>(request.ProjectRequestDto);

        projectToAdd.AuthorId = userId;
        projectToAdd.CreatedAt = DateTime.Now;
        projectToAdd.TeamMembers.Add(new TeamMember() { UserId = userId });

        var addedProject = await _projectRepository.AddAsync(projectToAdd);
        await _projectRepository.SaveAsync();

        var response = _mapper.Map<ProjectResponseDto>(addedProject);

        return response.Id;
    }
}
