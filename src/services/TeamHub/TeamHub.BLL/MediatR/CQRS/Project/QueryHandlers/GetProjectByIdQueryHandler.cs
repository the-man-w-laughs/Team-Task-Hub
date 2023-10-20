using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.Extensions;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Queries;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectRepository _projectRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly IMapper _mapper;

    public GetProjectByIdQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IProjectRepository projectRepository,
        ITeamMemberRepository teamMemberRepository,
        IMapper mapper
    )
    {
        _projectRepository = projectRepository;
        _teamMemberRepository = teamMemberRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ProjectResponseDto> Handle(
        GetProjectByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();

        var teamMember = await _teamMemberRepository.GetAsync(
            teamMember => teamMember.UserId == userId && teamMember.ProjectId == request.ProjectId
        );

        if (teamMember == null)
        {
            throw new ForbiddenException(
                $"User with id {userId} cannot get info about project with id {request.ProjectId}."
            );
        }

        var project = await _projectRepository.GetByIdAsync(request.ProjectId);

        if (project == null)
        {
            throw new NotFoundException($"Project with ID {request.ProjectId} not found.");
        }

        var response = _mapper.Map<ProjectResponseDto>(project);

        return response;
    }
}
