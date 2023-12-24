using Bogus;
using Identity.Application.Dtos;

namespace Identity.Tests.Fakers
{
    public class AppUserRegisterDtoFaker : Faker<AppUserRegisterDto>
    {
        public AppUserRegisterDtoFaker()
        {
            RuleFor(registerDto => registerDto.Email, faker => faker.Internet.Email());
            RuleFor(registerDto => registerDto.Password, faker => faker.Internet.Password());
        }
    }
}
