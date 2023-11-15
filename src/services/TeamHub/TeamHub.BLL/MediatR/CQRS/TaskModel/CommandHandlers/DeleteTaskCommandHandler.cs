using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using Shared.Extensions;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Commands;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, int>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ITaskModelRepository _taskRepository;

    public DeleteTaskCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ITaskModelRepository taskModelRepository
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _taskRepository = taskModelRepository;
    }

    public async Task<int> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.GetUserId();

        // Check if the requested task exists and current users team member exists.
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);

        if (task == null)
        {
            throw new NotFoundException($"Task with id {request.TaskId} was not found.");
        }

        if (userId != task.TeamMember.UserId && userId != task.Project.AuthorId)
        {
            throw new ForbiddenException(
                $"User with id {userId} cannot delete tesk with id {task.Id}."
            );
        }

        _taskRepository.Delete(task);
        await _taskRepository.SaveAsync(cancellationToken);

        return task.Id;
    }
}
