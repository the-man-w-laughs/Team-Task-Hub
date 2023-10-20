using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using TeamHub.BLL.Dtos;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Tasks.Queries;

public class GetCommentByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskModelResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITaskModelRepository _taskRepository;
    private readonly IMapper _mapper;

    public GetCommentByIdQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        ITaskModelRepository taskRepository,
        IMapper mapper
    )
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TaskModelResponseDto> Handle(
        GetTaskByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var project = await _taskRepository.GetByIdAsync(request.TaskId);

        if (project == null)
        {
            throw new NotFoundException($"Comment with ID {request.TaskId} not found.");
        }

        var response = _mapper.Map<TaskModelResponseDto>(project);

        return response;
    }
}
