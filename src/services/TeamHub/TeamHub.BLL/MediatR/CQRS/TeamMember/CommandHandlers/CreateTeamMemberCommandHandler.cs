using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using TeamHub.BLL.Extensions;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.MediatR.CQRS.TeamMembers.Commands;

public class CreateTeamMemberCommandHandler : IRequestHandler<CreateTeamMemberCommand, int>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ITeamMemberRepository _teamMemberRepository;
    private readonly IProjectRepository _projectRepository;

    public CreateTeamMemberCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ITeamMemberRepository teamMemberRepository,
        IProjectRepository projectRepository
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _teamMemberRepository = teamMemberRepository;
        _projectRepository = projectRepository;
    }

    public async Task<int> Handle(
        CreateTeamMemberCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor.GetUserId();

        await _projectRepository.GetProjectByIdAsync(request.ProjectId);
        await _teamMemberRepository.GetTeamMemberAsync(userId, request.ProjectId);

        var teamMember = await _teamMemberRepository.GetTeamMemberAsync(
            request.UserId,
            request.ProjectId
        );

        if (teamMember != null)
        {
            throw new WrongActionException(
                $"User with id {request.UserId} is already a part of project with id {request.ProjectId}."
            );
        }

        var teamMemberToAdd = new TeamMember()
        {
            ProjectId = request.ProjectId,
            UserId = request.UserId,
            CreatedAt = DateTime.Now
        };

        var addedTeamMember = await _teamMemberRepository.AddAsync(teamMemberToAdd);
        await _teamMemberRepository.SaveAsync();

        return addedTeamMember.Id;
    }
}
