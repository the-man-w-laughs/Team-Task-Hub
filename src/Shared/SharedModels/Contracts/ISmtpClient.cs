using System.Net.Mail;

namespace Shared.SharedModels.Contracts
{
    public interface ISmtpClient : IDisposable
    {
        public void Send(MailMessage message);
    }
}
