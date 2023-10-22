using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
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
        var userId = _httpContextAccessor.GetUserId();

        var project = await _projectRepository.GetProjectByIdAsync(request.ProjectId);
        await _teamMemberRepository.GetTeamMemberAsync(userId, request.ProjectId);

        var response = _mapper.Map<ProjectResponseDto>(project);

        return response;
    }
}
