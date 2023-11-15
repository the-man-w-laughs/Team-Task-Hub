namespace TeamHub.BLL.Dtos
{
    public class TaskModelRequestDto
    {
        public int PriorityId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime? Deadline { get; set; }
    }
}
