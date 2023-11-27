using Shared.Enums;

namespace TeamHub.BLL.Dtos
{
    public class TaskModelResponseDto
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public int ProjectId { get; set; }
        public TaskPriorityEnum PriorityId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime? Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<UserResponseDto> TasksHandlers { get; set; } =
            new List<UserResponseDto>();
    }
}
