using AutoMapper;
using Identity.Application.Ports.Services;
using Identity.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Shared.Exceptions;
using Shared.SharedModels;

namespace Identity.Application.Services
{
    public class EmailConfirmationService : IEmailService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPublishEndpoint _publishEndpoint;

        public EmailConfirmationService(
            IMapper mapper,
            UserManager<AppUser> userManager,
            IPublishEndpoint publishEndpoint
        )
        {
            _mapper = mapper;
            _userManager = userManager;
            _publishEndpoint = publishEndpoint;
        }

        public async Task ConfirmEmail(string token, string email)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(email))
            {
                throw new WrongActionException("Token or email is missing.");
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new NotFoundException($"User with email {email} not found.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                throw new WrongActionException("Failed to confirm email.");
            }

            var message = _mapper.Map<UserCreatedMessage>(user);

            await _publishEndpoint.Publish(message);
        }
    }
}
