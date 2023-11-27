using Amazon.Runtime.Internal.Util;
using AutoMapper;
using Hangfire;
using Identity.Application.Dtos;
using Identity.Application.Ports.Services;
using Identity.Application.Ports.Utils;
using Identity.Application.Result;
using Identity.Application.ResultPattern.Results;
using Identity.Domain.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.IdentityConstraints;
using Shared.SharedModels;

namespace Identity.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IConfirmationEmailSender _emailConfirmationHelper;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IMapper mapper,
            IAppUserRepository appUserRepository,
            IPublishEndpoint publishEndpoint,
            IConfirmationEmailSender emailConfirmationHelper,
            ILogger<UserService> logger
        )
        {
            _mapper = mapper;
            _appUserRepository = appUserRepository;
            _publishEndpoint = publishEndpoint;
            _emailConfirmationHelper = emailConfirmationHelper;
            _logger = logger;
        }

        public async Task<Result<string>> AddUserAsync(AppUserRegisterDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            var identityResult = await _appUserRepository.CreateUserAsync(
                appUser,
                appUserDto.Password
            );

            if (!identityResult.Succeeded)
            {
                _logger.LogInformation(
                    "Failed to create user with email {Email}.",
                    appUserDto.Email
                );

                return new InvalidResult<string>(
                    $"Failed to add: {string.Join(", ", identityResult.Errors.Select(e => e.Description))}"
                );
            }

            BackgroundJob.Enqueue(() => _emailConfirmationHelper.SendEmailAsync(appUser));

            _logger.LogInformation(
                "Confirmation email sent successfully to {Email}.",
                appUserDto.Email
            );

            return new SuccessResult<string>(
                $"Confirmation email sent successfully! Please checkout your inbox {appUserDto.Email}"
            );
        }

        public async Task<Result<List<AppUserDto>>> GetAllUsersAsync(int offset, int limit)
        {
            var users = await _appUserRepository.GetAllUsersAsync(offset, limit);

            var usersDtos = _mapper.Map<List<AppUserDto>>(users);

            return new SuccessResult<List<AppUserDto>>(usersDtos);
        }

        public async Task<Result<AppUserDto>> GetUserByIdAsync(int id)
        {
            var user = await _appUserRepository.GetUserByIdAsync(id.ToString());

            if (user == null)
            {
                return new InvalidResult<AppUserDto>("User not found.");
            }

            var userDto = _mapper.Map<AppUserDto>(user);

            return new SuccessResult<AppUserDto>(userDto);
        }

        public async Task<Result<AppUserDto>> DeleteUserByIdAsync(int id)
        {
            var user = await _appUserRepository.GetUserByIdAsync(id.ToString());

            if (user == null)
            {
                return new InvalidResult<AppUserDto>($"User with id \"{id}\" not found.");
            }

            if (await _appUserRepository.IsUserInRoleAsync(user, Roles.AdminRole.Name!))
            {
                return new InvalidResult<AppUserDto>("Admins cannot delete themselves.");
            }

            var identityResult = await _appUserRepository.DeleteUserAsync(user);

            if (!identityResult.Succeeded)
            {
                return new InvalidResult<AppUserDto>(
                    $"Failed to delete: {string.Join(", ", identityResult.Errors.Select(e => e.Description))}"
                );
            }

            var message = _mapper.Map<UserDeletedMessage>(user);

            await _publishEndpoint.Publish(message);

            var userDto = _mapper.Map<AppUserDto>(user);

            return new SuccessResult<AppUserDto>(userDto);
        }

        public async Task<Result<AppUserDto>> GetUserByEmailAsync(string email)
        {
            var user = await _appUserRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return new InvalidResult<AppUserDto>($"User with email \"{email}\" not found.");
            }

            var userDto = _mapper.Map<AppUserDto>(user);

            return new SuccessResult<AppUserDto>(userDto);
        }
    }
}
