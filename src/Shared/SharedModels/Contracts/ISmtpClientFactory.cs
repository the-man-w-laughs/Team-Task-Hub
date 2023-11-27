namespace Shared.SharedModels.Contracts
{
    public interface ISmtpClientFactory
    {
        public ISmtpClient CreateSmtpClient(string host, string email, string appPassword);
    }
}
