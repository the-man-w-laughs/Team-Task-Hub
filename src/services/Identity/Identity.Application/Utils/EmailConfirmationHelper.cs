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
        private readonly IUrlHelper _urlHelper;
        private readonly EmailCredentials _options;
        private readonly UserManager<AppUser> _userManager;

        public EmailConfirmationHelper(
            UserManager<AppUser> userManager,
            IUrlHelper urlHelper,
            IOptions<EmailCredentials> options
        )
        {
            _userManager = userManager;
            _urlHelper = urlHelper;
            _options = options.Value;
        }

        public async Task<bool> SendEmail(AppUser appUser)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
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
                // log exception
                return false;
            }
        }

        private string BuildConfirmationLink(string userEmail, string token)
        {
            return _urlHelper.Action(
                "confirm-email",
                "Email",
                new { token, email = userEmail },
                "http"
            )!;
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
