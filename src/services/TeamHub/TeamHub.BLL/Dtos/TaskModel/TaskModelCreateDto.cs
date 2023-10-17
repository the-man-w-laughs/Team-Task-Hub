namespace TeamHub.BLL.Dtos
{
    public class TaskModelCreateDto
    {
        public int ProjectsId { get; set; }
        public int PriorityId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime? Deadline { get; set; }
    }
}
