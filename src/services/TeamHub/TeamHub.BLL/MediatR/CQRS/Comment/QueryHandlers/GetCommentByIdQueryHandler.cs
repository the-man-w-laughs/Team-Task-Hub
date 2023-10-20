using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.MediatR.CQRS.Comments.Queries;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Projects.Queries;

public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, CommentResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public GetCommentByIdQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        ICommentRepository commentRepository,
        IMapper mapper
    )
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CommentResponseDto> Handle(
        GetCommentByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var project = await _commentRepository.GetByIdAsync(request.commentId);

        if (project == null)
        {
            throw new NotFoundException($"Comment with ID {request.commentId} not found.");
        }
        var response = _mapper.Map<CommentResponseDto>(project);

        return response;
    }
}
