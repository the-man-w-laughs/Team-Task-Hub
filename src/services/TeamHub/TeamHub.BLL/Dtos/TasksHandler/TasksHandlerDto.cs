using TeamHub.DAL.Models;

namespace TeamHub.BLL.Dtos
{
    public class TasksHandlerDto
    {
        public int Id { get; set; }
        public int TeamMembersId { get; set; }
        public int TasksId { get; set; }
        public virtual TaskModel Tasks { get; set; } = null!;
        public virtual TeamMember TeamMembers { get; set; } = null!;
    }
}
