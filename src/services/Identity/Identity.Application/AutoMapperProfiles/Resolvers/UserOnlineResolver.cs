using AutoMapper;
using Identity.Application.Dtos;
using Identity.Domain.Entities;
using Shared.Repository.NoSql.Redis;

namespace Identity.Application.AutoMapperProfiles.Resolvers
{
    public class UserOnlineResolver : IValueResolver<AppUser, AppUserDto, bool>
    {
        private readonly IUserRequestRepository _userRequestRepository;

        public UserOnlineResolver(IUserRequestRepository userRequestRepository)
        {
            _userRequestRepository = userRequestRepository;
        }

        public bool Resolve(
            AppUser source,
            AppUserDto destination,
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
