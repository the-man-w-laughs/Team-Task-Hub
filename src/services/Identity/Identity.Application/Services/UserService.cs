using AutoMapper;
using Identity.Application.Dtos;
using Identity.Application.Ports.Repositories;
using Identity.Application.Ports.Services;
using Identity.Application.Result;
using Identity.Application.ResultPattern.Results;
using Identity.Domain.Constraints;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuthDBContext _authDbContext;

        public UserService(
            IMapper mapper,
            UserManager<AppUser> userManager,
            IAuthDBContext authDbContext
        )
        {
            this._mapper = mapper;
            this._userManager = userManager;
            this._authDbContext = authDbContext;
        }

        public async Task<Result<int>> AddUserAsync(AppUserRegisterDto appUserDto)
        {
            var appUser = _mapper.Map<AppUser>(appUserDto);

            var identityResult = await _userManager.CreateAsync(appUser, appUserDto.Password);

            if (!identityResult.Succeeded)
            {
                var errorMessage = string.Join(
                    ", ",
                    identityResult.Errors.Select(e => e.Description)
                );
                return new InvalidResult<int>($"Unable to create user: {errorMessage}");
            }

            await _authDbContext.SaveChangesAsync();

            return new SuccessResult<int>(appUser.Id);
        }

        public async Task<Result<List<AppUserDto>>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var usersDtos = _mapper.Map<List<AppUserDto>>(users);

            return new SuccessResult<List<AppUserDto>>(usersDtos);
        }

        public async Task<Result<AppUserDto>> GetUserByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return new InvalidResult<AppUserDto>("User not found.");
            }

            var userDto = _mapper.Map<AppUserDto>(user);

            return new SuccessResult<AppUserDto>(userDto);
        }

        public async Task<Result<AppUserDto>> DeleteUserByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return new InvalidResult<AppUserDto>("User not found.");
            }

            if (await _userManager.IsInRoleAsync(user, Roles.AdminRole.Name!))
            {
                return new InvalidResult<AppUserDto>("Admins cannot delete themselves.");
            }

            var identityResult = await _userManager.DeleteAsync(user);

            if (!identityResult.Succeeded)
            {
                var errorMessage = string.Join(
                    ", ",
                    identityResult.Errors.Select(e => e.Description)
                );
                return new InvalidResult<AppUserDto>($"Unable to delete user: {errorMessage}");
            }

            await _authDbContext.SaveChangesAsync();

            var userDto = _mapper.Map<AppUserDto>(user);

            return new SuccessResult<AppUserDto>(userDto);
        }
    }
}
