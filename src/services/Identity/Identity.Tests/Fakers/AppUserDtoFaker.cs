using Bogus;
using Identity.Application.Dtos;

namespace Identity.Tests.Fakers
{
    public class AppUserDtoFaker : Faker<AppUserDto>
    {
        public AppUserDtoFaker()
        {
            RuleFor(u => u.Id, f => f.Random.Number(max: int.MaxValue));
            RuleFor(u => u.Email, f => f.Internet.Email());
            RuleFor(u => u.IsOnline, f => f.Random.Bool());
        }
    }
}
