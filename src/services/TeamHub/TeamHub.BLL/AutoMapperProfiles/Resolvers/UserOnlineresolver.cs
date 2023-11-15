using AutoMapper;
using Shared.Repository.NoSql.Redis;
using TeamHub.BLL.Dtos;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.AutoMapperProfiles.Resolvers
{
    public class UserOnlineResolver : IValueResolver<User, UserResponseDto, bool>
    {
        private readonly IUserRequestRepository _userRequestRepository;

        public UserOnlineResolver(IUserRequestRepository userRequestRepository)
        {
            _userRequestRepository = userRequestRepository;
        }

        public bool Resolve(
            User source,
            UserResponseDto destination,
            bool destMember,
            ResolutionContext context
        )
        {
            var latestRequestTime = _userRequestRepository.GetLatestRequestTime(
                source.Id.ToString()
            );

            return latestRequestTime != null;
        }
    }
}
