using System.Net.Mail;
using Identity.Application.Ports.Utils;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.SharedModels;

namespace Identity.Application.Utils
{
    public class EmailConfirmationHelper : IEmailConfirmationHelper
    {
        private readonly EmailCredentials _options;
        private readonly UserManager<AppUser> _userManager;

        public EmailConfirmationHelper(
            UserManager<AppUser> userManager,
            IOptions<EmailCredentials> options
        )
        {
            _userManager = userManager;
            _options = options.Value;
        }

        public async Task<bool> SendEmail(AppUser appUser)
        {
            //var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var token = "token";
            var targetEmail = appUser.Email!;
            var confirmationLink = BuildConfirmationLink(targetEmail, token);
            var mailMessage = CreateMailMessage(targetEmail, confirmationLink);

            try
            {
                using (var client = new SmtpClient(_options.Email, 80))
                {
                    client.Credentials = new System.Net.NetworkCredential(
                        _options.Email,
                        _options.Password
                    );
                    client.Send(mailMessage);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
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
                From = new MailAddress(_options.Email),
                To = { new MailAddress(toEmail) },
                Subject = "Confirm your email",
                IsBodyHtml = true,
                Body = body
            };
        }
    }
}
