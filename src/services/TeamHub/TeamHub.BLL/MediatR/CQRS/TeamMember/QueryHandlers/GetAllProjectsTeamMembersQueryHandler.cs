using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.Extensions;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.TeamMembers.Queries;

public class GetAllProjectsTeamMembersQueryHandler
    : IRequestHandler<GetAllProjectsTeamMembersQuery, IEnumerable<UserResponseDto>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectRepository _projectRepository;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly IMapper _mapper;

    public GetAllProjectsTeamMembersQueryHandler(
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

    public async Task<IEnumerable<UserResponseDto>> Handle(
        GetAllProjectsTeamMembersQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor.GetUserId();

        await _projectRepository.GetProjectByIdAsync(request.ProjectId);
        await _teamMemberRepository.GetTeamMemberAsync(userId, request.ProjectId);

        var teamMembers = await _teamMemberRepository.GetAllAsync(
            teamMember => teamMember.ProjectId == request.ProjectId
        );

        var usersResponseDto = teamMembers.Select(
            teamMember => _mapper.Map<UserResponseDto>(teamMember.User)
        );

        return usersResponseDto;
    }
}
