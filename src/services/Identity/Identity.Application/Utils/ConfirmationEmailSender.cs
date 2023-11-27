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
        private readonly IMailMessageBuilder _mailMessageBuilder;

        public ConfirmationEmailSender(
            UserManager<AppUser> userManager,
            IOptions<EmailCredentials> emailCredentialsOptions,
            IOptions<UriOptions> uriOptions,
            ISmtpClientFactory smtpClientFactory,
            IMailMessageBuilder mailMessageBuilder
        )
        {
            _userManager = userManager;
            _smtpClientFactory = smtpClientFactory;
            _mailMessageBuilder = mailMessageBuilder;
            _uriOptions = uriOptions.Value;
            _emailCredentials = emailCredentialsOptions.Value;
        }

        public async Task SendEmailAsync(AppUser appUser)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var targetEmail = appUser.Email!;
            var confirmationLink = BuildConfirmationLink(targetEmail, token);

            var mailMessage = _mailMessageBuilder.CreateConfirmationEmailMessage(
                _emailCredentials.Email,
                targetEmail,
                confirmationLink
            );

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
    }
}
