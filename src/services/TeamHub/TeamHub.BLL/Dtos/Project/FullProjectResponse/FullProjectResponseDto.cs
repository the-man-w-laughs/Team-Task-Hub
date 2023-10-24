namespace TeamHub.BLL.Dtos
{
    public class FullProjectResponseDto
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public UserResponseDto Creator { get; set; } = null!;
        public ICollection<UserResponseDto> TeamMembers { get; set; } = new List<UserResponseDto>();
        public ICollection<ProjectTaskResponseDto> Tasks { get; set; } =
            new List<ProjectTaskResponseDto>();
    }
}
