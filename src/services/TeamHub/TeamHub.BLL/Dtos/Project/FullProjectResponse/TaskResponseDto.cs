using TeamHub.DAL.Constraints;

namespace TeamHub.BLL.Dtos
{
    public class ProjectTaskResponseDto
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public TaskPriorityEnum PriorityId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime? Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<int> TasksHandlersIds { get; set; } = new List<int>();
    }
}
