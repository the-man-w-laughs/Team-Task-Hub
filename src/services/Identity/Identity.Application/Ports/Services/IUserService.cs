using Identity.Application.Dtos;
using Identity.Application.Result;

namespace Identity.Application.Ports.Services
{
    public interface IUserService
    {
        Task<Result<string>> AddUserAsync(AppUserRegisterDto appUserDto);
        Task<Result<AppUserDto>> DeleteUserByIdAsync(int id);
        Task<Result<List<AppUserDto>>> GetAllUsersAsync();
        Task<Result<AppUserDto>> GetUserByIdAsync(int id);
        Task<Result<AppUserDto>> GetUserByEmailAsync(string email);
    }
}
