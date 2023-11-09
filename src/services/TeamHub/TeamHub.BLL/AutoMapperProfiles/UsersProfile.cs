using Shared.SharedModels;
using TeamHub.BLL.Dtos;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.AutoMapperProfiles
{
    public class UsersProfile : BaseProfile
    {
        public UsersProfile()
        {
            CreateMap<User, UserResponseDto>();
            CreateMap<UserCreatedMessage, User>();
        }
    }
}
