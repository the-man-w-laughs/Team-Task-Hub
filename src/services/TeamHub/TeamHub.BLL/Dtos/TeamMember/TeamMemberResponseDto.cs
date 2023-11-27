namespace TeamHub.BLL.Dtos.TeamMember
{
    public class TeamMemberResponseDto
    {
        public string ProjectName { get; set; } = string.Empty;
        public List<ShortTaskModelResponseDto> Tasks { get; set; } = new();
    }
}
