using Shared.SharedModels.Contracts;

namespace Shared.SharedModels
{
    public class CustomSmtpClientFactory : ISmtpClientFactory
    {
        public ISmtpClient CreateSmtpClient(string host, string email, string appPassword)
        {
            return new CustomSmtpClient(host, email, appPassword);
        }
    }
}
