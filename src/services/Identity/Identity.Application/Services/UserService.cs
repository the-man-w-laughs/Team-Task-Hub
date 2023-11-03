using AutoMapper;
using Identity.Application.Dtos;
using Identity.Application.Ports.Services;
using Identity.Application.Result;
using Identity.Application.ResultPattern.Results;
using Identity.Domain.Entities;
using Shared.IdentityConstraints;

namespace Identity.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IAppUserRepository _appUserRepository;

        public UserService(IMapper mapper, IAppUserRepository appUserRepository)
        {
            _mapper = mapper;
            _appUserRepository = appUserRepository;
        }

        public async Task<Result<int>> AddUserAsync(AppUserRegisterDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            var identityResult = await _appUserRepository.CreateUserAsync(
                appUser,
                appUserDto.Password
            );

            if (!identityResult.Succeeded)
            {
                return new InvalidResult<int>(
                    $"Failed to add: {string.Join(", ", identityResult.Errors.Select(e => e.Description))}"
                );
            }

            return new SuccessResult<int>(appUser.Id);
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
                return new InvalidResult<AppUserDto>("User not found.");
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

            var userDto = _mapper.Map<AppUserDto>(user);

            return new SuccessResult<AppUserDto>(userDto);
        }
    }
}
