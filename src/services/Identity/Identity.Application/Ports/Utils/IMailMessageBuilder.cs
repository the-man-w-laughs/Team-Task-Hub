using System.Net.Mail;

namespace Identity.Application.Ports.Utils
{
    public interface IMailMessageBuilder
    {
        MailMessage CreateConfirmationEmailMessage(
            string sourceEmail,
            string targetEmail,
            string confirmaitonLink
        );
    }
}
