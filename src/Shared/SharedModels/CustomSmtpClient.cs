using System.Net.Mail;

namespace Shared.SharedModels
{
    public class CustomSmtpClient : SmtpClient
    {
        public CustomSmtpClient(string host, string email, string password)
            : base(host, 587)
        {
            EnableSsl = true;
            Credentials = new System.Net.NetworkCredential(email, password);
        }
    }
}
