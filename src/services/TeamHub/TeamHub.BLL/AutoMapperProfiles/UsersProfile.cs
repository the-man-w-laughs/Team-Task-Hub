using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Shared.Repository.NoSql.Redis;
using Shared.SharedModels;
using TeamHub.BLL.Dtos;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.AutoMapperProfiles
{
    public class UsersProfile : Profile
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersProfile(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            CreateMap<User, UserResponseDto>()
                .AfterMap((user, userResponseDto, context) => SetUserOnlineStatus(userResponseDto));

            CreateMap<UserCreatedMessage, User>();
        }

        private void SetUserOnlineStatus(UserResponseDto userResponseDto)
        {
            var userRequestRepository =
                _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IUserRequestRepository>();

            var latestRequestTime = userRequestRepository.GetLatestRequestTime(
                userResponseDto.Id.ToString()
            );

            userResponseDto.IsOnline = latestRequestTime != null;
        }
    }
}
