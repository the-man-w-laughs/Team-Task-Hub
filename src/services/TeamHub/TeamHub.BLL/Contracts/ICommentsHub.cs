using TeamHub.BLL.Dtos;

namespace TeamHub.BLL.Contracts
{
    public interface ICommentsHub
    {
        Task Connection(string message);
        Task Connection(TaskModelResponseDto task);
        Task CreateComment(CommentResponseDto commentResponseDto);
        Task UpdateComment(CommentResponseDto commentResponseDto);
        Task DeleteComment(CommentResponseDto commentResponseDto);
    }
}
