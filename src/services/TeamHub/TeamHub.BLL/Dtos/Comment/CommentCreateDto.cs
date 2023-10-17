namespace TeamHub.BLL.Dtos
{
    public class CommentCreateDto
    {
        public int TasksId { get; set; }

        public string Content { get; set; } = null!;
    }
}
