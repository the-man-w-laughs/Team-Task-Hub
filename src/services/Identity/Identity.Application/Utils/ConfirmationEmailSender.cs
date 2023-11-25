using System.Net.Mail;
using Identity.Application.Ports.Utils;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared.SharedModels;
using Shared.SharedModels.Contracts;

namespace Identity.Application.Utils
{
    public class ConfirmationEmailSender : IConfirmationEmailSender
    {
        private readonly EmailCredentials _emailCredentials;
        private readonly UriOptions _uriOptions;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISmtpClientFactory _smtpClientFactory;

        public ConfirmationEmailSender(
            UserManager<AppUser> userManager,
            IOptions<EmailCredentials> emailCredentialsOptions,
            IOptions<UriOptions> uriOptions,
            ISmtpClientFactory smtpClientFactory
        )
        {
            _userManager = userManager;
            _smtpClientFactory = smtpClientFactory;
            _uriOptions = uriOptions.Value;
            _emailCredentials = emailCredentialsOptions.Value;
        }

        public async Task SendEmailAsync(AppUser appUser)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var targetEmail = appUser.Email!;
            var confirmationLink = BuildConfirmationLink(targetEmail, token);

            var Body = CreateBody(confirmationLink);

            var mailMessage = CreateMailMessage(targetEmail, Body);

            using (
                var client = _smtpClientFactory.CreateSmtpClient(
                    _emailCredentials.Host,
                    _emailCredentials.Email,
                    _emailCredentials.AppPassword
                )
            )
            {
                client.Send(mailMessage);
            }
        }

        private string BuildConfirmationLink(string userEmail, string token)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = _uriOptions.Scheme,
                Host = _uriOptions.Host,
                Path = _uriOptions.Path,
                Port = _uriOptions.Port,
                Query =
                    $"token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(userEmail)}"
            };

            return uriBuilder.Uri.ToString();
        }

        private MailMessage CreateMailMessage(string toEmail, string body)
        {
            return new MailMessage
            {
                From = new MailAddress(_emailCredentials.Email, "TeamTaskHub"),
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
