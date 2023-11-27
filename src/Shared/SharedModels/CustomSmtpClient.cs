using System.Net.Mail;
using Shared.SharedModels.Contracts;

namespace Shared.SharedModels
{
    public class CustomSmtpClient : SmtpClient, ISmtpClient
    {
        public CustomSmtpClient(string host, string email, string password)
            : base(host, 587)
        {
            EnableSsl = true;
            Credentials = new System.Net.NetworkCredential(email, password);
        }
    }
}
