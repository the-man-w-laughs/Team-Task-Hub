using Bogus;
using Identity.Application.Dtos;

namespace Identity.Tests.Fakers
{
    public class AppUserRegisterDtoFaker : Faker<AppUserRegisterDto>
    {
        public AppUserRegisterDtoFaker()
        {
            RuleFor(u => u.Email, f => f.Internet.Email());
            RuleFor(u => u.Password, f => f.Internet.Password());
        }
    }
}
