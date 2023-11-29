using AutoMapper;
using Hangfire;
using Identity.Application.Dtos;
using Identity.Application.Ports.Services;
using Identity.Application.Ports.Utils;
using Identity.Application.Result;
using Identity.Application.ResultPattern.Results;
using Identity.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.IdentityConstraints;
using Shared.SharedModels;
using Shared.Extensions;

namespace Identity.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IConfirmationEmailSender _emailConfirmationHelper;
        private readonly ILogger<UserService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            IMapper mapper,
            IAppUserRepository appUserRepository,
            IPublishEndpoint publishEndpoint,
            IConfirmationEmailSender emailConfirmationHelper,
            ILogger<UserService> logger,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _mapper = mapper;
            _appUserRepository = appUserRepository;
            _publishEndpoint = publishEndpoint;
            _emailConfirmationHelper = emailConfirmationHelper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<string>> AddUserAsync(AppUserRegisterDto appUserDto)
        {
            _logger.LogInformation(
                "Attempting to create a new user with email {Email}.",
                appUserDto.Email
            );

            var appUser = _mapper.Map<AppUser>(appUserDto);

            var identityResult = await _appUserRepository.CreateUserAsync(
                appUser,
                appUserDto.Password
            );

            if (!identityResult.Succeeded)
            {
                _logger.LogError(
                    "Failed to create user with email {Email}. Errors: {Errors}",
                    appUserDto.Email,
                    string.Join(", ", identityResult.Errors.Select(e => e.Description))
                );

                return new InvalidResult<string>(
                    $"Failed to add: {string.Join(", ", identityResult.Errors.Select(e => e.Description))}"
                );
            }

            BackgroundJob.Enqueue(() => _emailConfirmationHelper.SendEmailAsync(appUser));

            _logger.LogInformation(
                "User created successfully. Sending confirmation email to {Email}.",
                appUserDto.Email
            );

            return new SuccessResult<string>(
                $"Confirmation email sent successfully! Please check your inbox {appUserDto.Email}"
            );
        }

        public async Task<Result<List<AppUserDto>>> GetAllUsersAsync(int offset, int limit)
        {
            var userId = _httpContextAccessor.GetUserId();

            _logger.LogInformation(
                "User with ID {UserId} is attempting to retrieve all users with offset {Offset} and limit {Limit}.",
                userId,
                offset,
                limit
            );

            var users = await _appUserRepository.GetAllUsersAsync(offset, limit);

            if (users == null || !users.Any())
            {
                _logger.LogInformation(
                    "No users found for user with ID {UserId}. Returning an empty list.",
                    userId
                );

                return new SuccessResult<List<AppUserDto>>(new List<AppUserDto>());
            }

            var usersDtos = _mapper.Map<List<AppUserDto>>(users);

            _logger.LogInformation(
                "User with ID {UserId} successfully retrieved {UserCount} users.",
                userId,
                users.Count()
            );

            return new SuccessResult<List<AppUserDto>>(usersDtos);
        }

        public async Task<Result<AppUserDto>> GetUserByIdAsync(int targetUserId)
        {
            var userId = _httpContextAccessor.GetUserId();

            _logger.LogInformation(
                "User with ID {UserId} is attempting to retrieve info about user with ID {TargetUserId}.",
                userId,
                targetUserId
            );

            var user = await _appUserRepository.GetUserByIdAsync(targetUserId.ToString());

            if (user == null)
            {
                _logger.LogInformation(
                    "User with ID {UserId} failed to retrieve info about non-existent user with ID {TargetUserId}.",
                    userId,
                    targetUserId
                );

                return new InvalidResult<AppUserDto>("User not found.");
            }

            var userDto = _mapper.Map<AppUserDto>(user);

            _logger.LogInformation(
                "User with ID {UserId} successfully retrieved info about user with ID {TargetUserId}.",
                userId,
                targetUserId
            );

            return new SuccessResult<AppUserDto>(userDto);
        }

        public async Task<Result<AppUserDto>> DeleteUserByIdAsync(int targetUserId)
        {
            var currentUserId = _httpContextAccessor.GetUserId();
            _logger.LogInformation(
                "User with ID {CurrentUserId} is attempting to delete user with ID {TargetUserId}.",
                currentUserId,
                targetUserId
            );

            var user = await _appUserRepository.GetUserByIdAsync(targetUserId.ToString());

            if (user == null)
            {
                _logger.LogInformation(
                    "User with ID {CurrentUserId} failed to delete user with ID {TargetUserId}: target user not found.",
                    currentUserId,
                    targetUserId
                );

                return new InvalidResult<AppUserDto>($"User with ID \"{targetUserId}\" not found.");
            }

            if (await _appUserRepository.IsUserInRoleAsync(user, Roles.AdminRole.Name!))
            {
                _logger.LogInformation(
                    "User with ID {CurrentUserId} failed to delete user with ID {TargetUserId}: admins cannot delete themselves.",
                    currentUserId,
                    targetUserId
                );

                return new InvalidResult<AppUserDto>("Admins cannot delete themselves.");
            }

            var identityResult = await _appUserRepository.DeleteUserAsync(user);

            if (!identityResult.Succeeded)
            {
                _logger.LogError(
                    "User with ID {CurrentUserId} failed to delete user with ID {TargetUserId}. Errors: {Errors}",
                    currentUserId,
                    targetUserId,
                    string.Join(", ", identityResult.Errors.Select(e => e.Description))
                );

                return new InvalidResult<AppUserDto>(
                    $"Failed to delete: {string.Join(", ", identityResult.Errors.Select(e => e.Description))}"
                );
            }

            var message = _mapper.Map<UserDeletedMessage>(user);
            await _publishEndpoint.Publish(message);

            var userDto = _mapper.Map<AppUserDto>(user);

            _logger.LogInformation(
                "User with ID {CurrentUserId} successfully deleted user with ID {TargetUserId}.",
                currentUserId,
                targetUserId
            );

            return new SuccessResult<AppUserDto>(userDto);
        }

        public async Task<Result<AppUserDto>> GetUserByEmailAsync(string email)
        {
            var userId = _httpContextAccessor.GetUserId();
            _logger.LogInformation(
                "User with ID {UserId} is attempting to retrieve info about user with Email {Email}.",
                userId,
                email
            );

            var user = await _appUserRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                _logger.LogInformation(
                    "User with ID {UserId} failed to retrieve info about non-existent user with Email {Email}.",
                    userId,
                    email
                );

                return new InvalidResult<AppUserDto>($"User with Email \"{email}\" not found.");
            }

            var userDto = _mapper.Map<AppUserDto>(user);

            _logger.LogInformation(
                "User with ID {UserId} successfully retrieved info about user with Email {Email}.",
                userId,
                email
            );

            return new SuccessResult<AppUserDto>(userDto);
        }
    }
}
