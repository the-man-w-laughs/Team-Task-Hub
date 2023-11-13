using Identity.Application.AutoMapperProfiles.Resolvers;
using Identity.Application.Dtos;
using Identity.Domain.Entities;
using Shared.SharedModels;

namespace Identity.Application.AutoMapperProfiles
{
    public class UsersProfile : BaseProfile
    {
        public UsersProfile()
        {
            CreateMap<AppUserRegisterDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<AppUser, AppUserDto>()
                .ForMember(dest => dest.IsOnline, opt => opt.MapFrom<UserOnlineResolver>());

            CreateMap<AppUser, UserCreatedMessage>();
            CreateMap<AppUser, UserDeletedMessage>();
        }
    }
}
