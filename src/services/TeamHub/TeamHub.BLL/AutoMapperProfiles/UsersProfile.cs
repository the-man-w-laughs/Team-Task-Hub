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
            CreateMap<UserCreatedMessage, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(user => user.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(user => user.Email))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(user => user.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(user => user.Email))
                .ForMember(dest => dest.IsOnline, opt => opt.MapFrom<UserOnlineResolver>());

            CreateMap<User, UserDataContract>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(user => user.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(user => user.Email))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(user => user.CreatedAt));
        }
    }
}
