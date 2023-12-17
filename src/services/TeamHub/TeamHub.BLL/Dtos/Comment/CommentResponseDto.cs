namespace TeamHub.BLL.Dtos
{
    public class CommentResponseDto
    {
        public int Id { get; set; }
        public UserResponseDto Author { get; set; }
        public int TasksId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
