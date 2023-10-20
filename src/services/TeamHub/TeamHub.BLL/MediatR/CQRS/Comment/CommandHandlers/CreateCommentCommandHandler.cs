using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Extensions;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.MediatR.CQRS.Comments.Commands;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, int>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ICommentRepository _commentRepository;

    public CreateCommentCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ICommentRepository commentRepository
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _commentRepository = commentRepository;
    }

    public async Task<int> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = (_httpContextAccessor?.HttpContext?.User.GetUserId())!.Value;

        var commentToAdd = _mapper.Map<Comment>(request.CommentRequestDto);

        commentToAdd.AuthorId = userId;
        commentToAdd.CreatedAt = DateTime.Now;

        var addedComment = await _commentRepository.AddAsync(commentToAdd);
        await _commentRepository.SaveAsync();

        return addedComment.Id;
    }
}
