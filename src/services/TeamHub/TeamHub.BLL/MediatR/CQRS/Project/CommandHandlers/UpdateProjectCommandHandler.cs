using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using Shared.Extensions;
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
        var userId = _httpContextAccessor.GetUserId();
        ;

        var project = await _projectRepository.GetProjectByIdAsync(
            request.ProjectId,
            cancellationToken
        );

        if (userId != project.AuthorId)
        {
            throw new ForbiddenException(
                $"User with id {userId} doesn't have rights to alter project with id {project.Id}."
            );
        }

        _mapper.Map(request.ProjectRequestDto, project);

        _projectRepository.Update(project);
        await _projectRepository.SaveAsync(cancellationToken);

        return project.Id;
    }
}
