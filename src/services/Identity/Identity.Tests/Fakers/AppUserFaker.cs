using Bogus;
using Identity.Domain.Entities;

namespace Identity.Tests.Fakers
{
    public class AppUserFaker : Faker<AppUser>
    {
        public AppUserFaker()
        {
            RuleFor(appUser => appUser.Id, faker => faker.Random.Number(max: int.MaxValue));
            RuleFor(appUser => appUser.Email, faker => faker.Internet.Email());
        }
    }
}
