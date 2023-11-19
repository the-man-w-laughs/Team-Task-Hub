namespace TeamHub.BLL.Contracts
{
    public interface IMailingService
    {
        public Task SendPendingTasks();
    }
}
