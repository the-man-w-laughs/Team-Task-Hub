using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.Contracts
{
    public interface ICommentsHub
    {
        Task ConnectionAsync(string message);
        Task ConnectionAsync(TaskModelResponseDto task);
        Task CreateCommentAsync(CommentResponseDto commentResponseDto);
        Task UpdateCommentAsync(CommentResponseDto commentResponseDto);
        Task DeleteCommentAsync(CommentResponseDto commentResponseDto);
    }
}
