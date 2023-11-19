using Shared.Enums;

namespace TeamHub.BLL.Dtos
{
    public class ShortTaskModelResponseDto
    {
        public int Id { get; set; }
        public TaskPriorityEnum PriorityId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime? Deadline { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
