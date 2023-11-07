using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using Shared.Extensions;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Commands;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, int>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ITaskModelRepository _taskModelRepository;

    public UpdateTaskCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ITaskModelRepository taskRepository
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _taskModelRepository = taskRepository;
    }

    public async Task<int> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.GetUserId();

        var task = await _taskModelRepository.GetTaskByIdAsync(request.TaskId, cancellationToken);

        if (userId != task.TeamMember.UserId)
        {
            throw new ForbiddenException(
                $"User with id {userId} doesnt have rights to alter comment with id {task.Id}."
            );
        }

        _mapper.Map(request.TaskModelRequestDto, task);

        _taskModelRepository.Update(task);
        await _taskModelRepository.SaveAsync(cancellationToken);

        return task.Id;
    }
}
