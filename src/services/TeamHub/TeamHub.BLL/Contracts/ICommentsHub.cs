namespace TeamHub.BLL.Contracts
{
    public interface ICommentsHub
    {
        Task Connection(string message);
        Task NewComment(string message);
    }
}
