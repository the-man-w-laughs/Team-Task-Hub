using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Identity.Application.Dtos;
using Identity.Domain.Entities;
using Moq;

namespace Identity.Tests.Helpers
{
    public class MapperHelper
    {
        private readonly Mock<IMapper> _mapperMock;

        public MapperHelper(Mock<IMapper> mapperMock)
        {
            _mapperMock = mapperMock;
        }

        public void SetupMap(AppUserDto appUserDto, AppUser appUser)
        {
            _mapperMock.Setup(mapper => mapper.Map<AppUser>(appUserDto)).Returns(appUser);
        }

        public void SetupMap(AppUserRegisterDto appUserDto, AppUser appUser)
        {
            _mapperMock.Setup(mapper => mapper.Map<AppUser>(appUserDto)).Returns(appUser);
        }

        public void SetupMap(List<AppUser> appUsers, List<AppUserDto> appUserDtos)
        {
            _mapperMock
                .Setup(mapper => mapper.Map<List<AppUserDto>>(appUsers))
                .Returns(appUserDtos);
        }

        public void SetupMap(AppUser appUser, AppUserDto appUserDto)
        {
            _mapperMock.Setup(mapper => mapper.Map<AppUserDto>(appUser)).Returns(appUserDto);
        }
    }
}
