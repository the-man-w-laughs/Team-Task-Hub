using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using TeamHub.BLL.Extensions;
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
        var userId = (_httpContextAccessor?.HttpContext?.User.GetUserId())!.Value;

        var existingTask = await _taskModelRepository.GetByIdAsync(request.TaskId);

        if (existingTask == null)
        {
            throw new NotFoundException($"Cannot find comment with id {request.TaskId}.");
        }

        if (userId != existingTask.TeamMember.UserId)
        {
            throw new ForbiddenException(
                $"User with id {userId} cannot delete comment with id {existingTask.Id}."
            );
        }

        _mapper.Map(request.TaskModelRequestDto, existingTask);

        _taskModelRepository.Update(existingTask);
        await _taskModelRepository.SaveAsync();

        return existingTask.Id;
    }
}
