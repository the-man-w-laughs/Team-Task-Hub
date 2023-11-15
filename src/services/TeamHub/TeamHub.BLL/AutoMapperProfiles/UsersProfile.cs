using Shared.SharedModels;
using Shared.gRPC.FullProjectResponse;
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
            CreateMap<User, UserResponseDto>();
            CreateMap<User, UserDataContract>();
        }
    }
}
