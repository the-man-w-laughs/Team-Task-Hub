using Bogus;
using TeamHub.DAL.Models;

namespace TeamHub.Tests.Fakers
{
    public class UserFaker : Faker<User>
    {
        public UserFaker()
        {
            RuleFor(user => user.Id, faker => faker.IndexFaker);
            RuleFor(user => user.Email, (faker, user) => faker.Internet.Email());
            RuleFor(user => user.CreatedAt, faker => faker.Date.Past());
        }
    }
}
