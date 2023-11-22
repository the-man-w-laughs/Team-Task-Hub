using TeamHub.BLL.Dtos;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.Contracts
{
    public interface ICommentsHub
    {
        Task Connection(string message);
        Task CreateComment(CommentResponseDto commentResponseDto);
        Task UpdateComment(CommentResponseDto commentResponseDto);
        Task DeleteComment(CommentResponseDto commentResponseDto);
    }
}
