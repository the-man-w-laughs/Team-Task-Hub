namespace TeamHub.BLL.Dtos.TeamMember
{
    public class TeamMemberResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
