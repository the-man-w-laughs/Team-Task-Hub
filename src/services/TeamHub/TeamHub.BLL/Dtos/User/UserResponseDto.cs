namespace TeamHub.BLL.Dtos
{
    public partial class UserResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public bool IsOnline { get; set; }
    }
}
