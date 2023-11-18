using System.Net.Mail;
using Identity.Application.Ports.Utils;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared.SharedModels;

namespace Identity.Application.Utils
{
    public class ConfirmationEmailSender : IConfirmationEmailSender
    {
        private readonly EmailCredentials _options;
        private readonly UserManager<AppUser> _userManager;

        public ConfirmationEmailSender(
            UserManager<AppUser> userManager,
            IOptions<EmailCredentials> options
        )
        {
            _userManager = userManager;
            _options = options.Value;
        }

        public async Task SendEmail(AppUser appUser)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var targetEmail = appUser.Email!;
            var confirmationLink = BuildConfirmationLink(targetEmail, token);

            string Body = CreateBody(confirmationLink);

            var mailMessage = CreateMailMessage(targetEmail, Body);

            using (var client = new SmtpClient(host: _options.Host, port: 587))
            {
                client.Credentials = new System.Net.NetworkCredential(
                    _options.Email,
                    _options.AppPassword
                );
                client.EnableSsl = true;
                client.Send(mailMessage);
            }
        }

        private string BuildConfirmationLink(string userEmail, string token)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = "http",
                Host = "localhost",
                Path = "api/Email/confirm-email",
                Port = 5000,
                Query =
                    $"token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(userEmail)}"
            };

            return uriBuilder.Uri.ToString();
        }

        private MailMessage CreateMailMessage(string toEmail, string body)
        {
            return new MailMessage
            {
                From = new MailAddress(_options.Email, "TeamTaskHub"),
                To = { new MailAddress(toEmail) },
                Subject = "Confirm your email for teamtaskhub.",
                IsBodyHtml = true,
                Body = body
            };
        }

        private static string CreateBody(string uri)
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
