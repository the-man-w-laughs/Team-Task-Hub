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
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserName));

            CreateMap<AppUser, UserCreatedMessage>();
            CreateMap<AppUser, UserDeletedMessage>();
        }
    }
}
