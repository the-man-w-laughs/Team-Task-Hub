using Bogus;
using TeamHub.DAL.Models;

namespace TeamHub.Tests.Fakers
{
    public class CommentFaker : Faker<Comment>
    {
        public CommentFaker()
        {
            RuleFor(comment => comment.Id, faker => faker.IndexFaker);
            RuleFor(comment => comment.AuthorId, faker => faker.Random.Number());
            RuleFor(comment => comment.TasksId, faker => faker.Random.Number());
            RuleFor(comment => comment.Content, faker => faker.Lorem.Sentence());
            RuleFor(comment => comment.CreatedAt, faker => faker.Date.Past());
        }
    }
}
