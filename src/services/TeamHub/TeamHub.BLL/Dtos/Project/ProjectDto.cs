using TeamHub.DAL.Models;

namespace TeamHub.BLL.Dtos
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public virtual User Creator { get; set; } = null!;
        public virtual ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
        public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
    }
}
