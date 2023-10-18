using TeamHub.DAL.Models;

namespace TeamHub.BLL.Dtos
{
    public class CommentResponseDto
    {
        public string Id { get; set; } = null!;
        public int UsersId { get; set; }
        public int TasksId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public virtual TaskModelResponseDto Tasks { get; set; } = null!;
        public virtual User Users { get; set; } = null!;
    }
}
