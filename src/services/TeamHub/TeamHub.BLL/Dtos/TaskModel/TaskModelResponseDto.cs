using TeamHub.DAL.Constraints;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.Dtos
{
    public class TaskModelResponseDto
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public int ProjectsId { get; set; }
        public TaskPriorityEnum PriorityId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime? Deadline { get; set; }
        public sbyte IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual TeamMember Creator { get; set; } = null!;
        public virtual Project Projects { get; set; } = null!;
        public virtual ICollection<User> TasksHandlers { get; set; } = new List<User>();
    }
}
