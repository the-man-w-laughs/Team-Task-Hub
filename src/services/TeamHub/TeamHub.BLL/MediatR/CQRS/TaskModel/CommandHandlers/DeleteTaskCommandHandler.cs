using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using TeamHub.BLL.Extensions;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Commands;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, int>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ITaskModelRepository _taskModelRepository;

    public DeleteTaskCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ITaskModelRepository taskModelRepository
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _taskModelRepository = taskModelRepository;
    }

    public async Task<int> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();
        var task = await _taskModelRepository.GetByIdAsync(request.TaskId);

        if (task == null)
        {
            throw new NotFoundException($"Cannot find task with id {request.TaskId}");
        }

        if (userId != task.TeamMember.UserId && userId != task.Projects.AuthorId)
        {
            throw new ForbiddenException(
                $"User with id {userId} cannot delete tesk with id {task.Id}."
            );
        }

        _taskModelRepository.Delete(task);
        await _taskModelRepository.SaveAsync();

        return task.Id;
    }
}
