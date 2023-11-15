using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using Shared.Extensions;
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
        _mapper = mapper;
        _commentRepository = commentRepository;
    }

    public async Task<int> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.GetUserId();
        ;

        var comment = await _commentRepository.GetCommentByIdAsync(request.CommentId);

        if (userId != comment.AuthorId)
        {
            throw new ForbiddenException(
                $"User with id {userId} doesn't have rights to alter comment with id {comment.Id}."
            );
        }

        _mapper.Map(request.CommentRequestDto, comment);

        _commentRepository.Update(comment);
        await _commentRepository.SaveAsync();

        return comment.Id;
    }
}
