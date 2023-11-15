using Shared.SharedModels;
using TeamHub.BLL.Dtos;
using TeamHub.DAL.Models;
using TeamHub.BLL.AutoMapperProfiles.Resolvers;

namespace TeamHub.BLL.AutoMapperProfiles
{
    public class UsersProfile : BaseProfile
    {
        public UsersProfile()
        {
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.IsOnline, opt => opt.MapFrom<UserOnlineResolver>());
            CreateMap<UserCreatedMessage, User>();
        }
    }
}
