using Bogus;
using TeamHub.DAL.Models;

namespace TeamHub.Tests.Fakers
{
    public class UserFaker : Faker<User>
    {
        public UserFaker()
        {
            RuleFor(u => u.Id, f => f.IndexFaker);
            RuleFor(u => u.Email, (f, u) => f.Internet.Email());
            RuleFor(u => u.CreatedAt, f => f.Date.Past());
        }
    }
}
