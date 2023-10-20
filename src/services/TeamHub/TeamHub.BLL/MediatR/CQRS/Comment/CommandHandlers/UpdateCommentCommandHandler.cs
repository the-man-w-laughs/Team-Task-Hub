using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using TeamHub.BLL.Extensions;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Comments.Commands;

public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, int>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ICommentRepository _commentRepository;

    public UpdateCommentCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ICommentRepository commentRepository
    )
    {
        _httpContextAccessor = httpContextAccessor;
        this._mapper = mapper;
        this._commentRepository = commentRepository;
    }

    public async Task<int> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = (_httpContextAccessor?.HttpContext?.User.GetUserId())!.Value;

        var existingComment = await _commentRepository.GetByIdAsync(request.CommentId);

        if (existingComment == null)
        {
            throw new NotFoundException($"Cannot find comment with id {request.CommentId}.");
        }

        if (userId != existingComment.AuthorId)
        {
            throw new ForbiddenException(
                $"User with id {userId} cannot delete comment with id {existingComment.Id}."
            );
        }

        _mapper.Map(request.CommentRequestDto, existingComment);

        _commentRepository.Update(existingComment);
        await _commentRepository.SaveAsync();

        return existingComment.Id;
    }
}
