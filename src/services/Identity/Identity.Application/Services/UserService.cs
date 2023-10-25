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

            try
            {
                await _appUserRepository.CreateUserAsync(appUser, appUserDto.Password);
            }
            catch (Exception ex)
            {
                return new InvalidResult<int>(ex.Message);
            }

            return new SuccessResult<int>(appUser.Id);
        }

        public async Task<Result<List<AppUserDto>>> GetAllUsersAsync()
        {
            var users = await _appUserRepository.GetAllUsersAsync();

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

            try
            {
                await _appUserRepository.DeleteUserAsync(user);
            }
            catch (Exception ex)
            {
                return new InvalidResult<AppUserDto>(ex.Message);
            }

            var userDto = _mapper.Map<AppUserDto>(user);

            return new SuccessResult<AppUserDto>(userDto);
        }
    }
}
