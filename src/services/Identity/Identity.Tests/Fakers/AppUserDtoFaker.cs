using Bogus;
using Identity.Application.Dtos;

namespace Identity.Tests.Fakers
{
    public class AppUserDtoFaker : Faker<AppUserDto>
    {
        public AppUserDtoFaker()
        {
            RuleFor(userDto => userDto.Id, faker => faker.Random.Number(max: int.MaxValue));
            RuleFor(userDto => userDto.Email, faker => faker.Internet.Email());
            RuleFor(userDto => userDto.IsOnline, faker => faker.Random.Bool());
        }
    }
}
