using AutoMapper;
using Identity.Application.Ports.Services;
using Identity.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Shared.SharedModels;

namespace Identity.Application.Services
{
    public class EmailConfirmationService : IEmailConfirmationService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<EmailConfirmationService> _logger;

        public EmailConfirmationService(
            IMapper mapper,
            UserManager<AppUser> userManager,
            IPublishEndpoint publishEndpoint,
            ILogger<EmailConfirmationService> logger
        )
        {
            _mapper = mapper;
            _userManager = userManager;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task ConfirmEmail(string token, string email)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Token or email is missing.");
                throw new WrongActionException("Token or email is missing.");
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogInformation("User with email {Email} not found.", email);
                throw new NotFoundException($"User with email {email} not found.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                _logger.LogInformation("Failed to confirm email {Email}.", email);
                throw new WrongActionException($"Failed to confirm email {email}.");
            }

            var message = _mapper.Map<UserCreatedMessage>(user);
            await _publishEndpoint.Publish(message);

            _logger.LogInformation("Email {Email} is confirmed successfully.", email);
        }
    }
}
