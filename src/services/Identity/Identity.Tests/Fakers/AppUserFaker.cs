using Bogus;
using Identity.Domain.Entities;

namespace Identity.Tests.Fakers
{
    public class AppUserFaker : Faker<AppUser>
    {
        public AppUserFaker()
        {
            RuleFor(u => u.Id, f => f.Random.Number(max: int.MaxValue));
            RuleFor(u => u.Email, f => f.Internet.Email());
        }
    }
}
