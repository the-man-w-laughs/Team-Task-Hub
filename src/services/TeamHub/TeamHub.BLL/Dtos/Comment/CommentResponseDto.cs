namespace TeamHub.BLL.Dtos
{
    public class CommentResponseDto
    {
        public string Id { get; set; } = null!;
        public int UsersId { get; set; }
        public int TasksId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
