namespace TeamHub.BLL.Dtos.TaskHandler
{
    public partial class TaskHandlerResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TaskId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
