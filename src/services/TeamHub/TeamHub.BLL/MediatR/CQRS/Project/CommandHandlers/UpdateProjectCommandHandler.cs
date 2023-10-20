using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using TeamHub.BLL.Extensions;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Commands;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, int>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public UpdateProjectCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IProjectRepository projectRepository,
        IMapper mapper
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var userId = (_httpContextAccessor?.HttpContext?.User.GetUserId())!.Value;

        var existingProject = await _projectRepository.GetByIdAsync(request.ProjectId);

        if (existingProject == null)
        {
            throw new NotFoundException($"Cannot find project with id {request.ProjectId}.");
        }

        if (userId != existingProject.AuthorId)
        {
            throw new ForbiddenException(
                $"User with id {userId} cannot delete project with id {existingProject.Id}."
            );
        }

        _mapper.Map(request.ProjectRequestDto, existingProject);

        _projectRepository.Update(existingProject);
        await _projectRepository.SaveAsync();

        return existingProject.Id;
    }
}
