using System.Net.Mail;
using Identity.Application.Ports.Utils;

namespace Identity.Application.Utils
{
    public class MailMessageBuilder : IMailMessageBuilder
    {
        public MailMessage CreateConfirmationEmailMessage(
            string sourceEmail,
            string targetEmail,
            string confirmaitonLink
        )
        {
            var body = CreateBody(confirmaitonLink);

            return new MailMessage
            {
                From = new MailAddress(sourceEmail, "TeamTaskHub"),
                To = { new MailAddress(targetEmail) },
                Subject = "Confirm your email for teamtaskhub.",
                IsBodyHtml = true,
                Body = body
            };
        }

        private string CreateBody(string uri)
        {
            return $@"
            <html>
                <body>
                    <p>Dear User,</p>
                    <p>Thank you for choosing TeamTaskHub! To complete your registration, please click the link below:</p>
                    <a href='{uri}'>Confirm Email Address</a>
                    <p>If you didn't create an account with TeamTaskHub, you can safely ignore this email.</p>
                    <p>Best regards,<br/>The TeamTaskHub Team</p>
                </body>
            </html>";
        }
    }
}
