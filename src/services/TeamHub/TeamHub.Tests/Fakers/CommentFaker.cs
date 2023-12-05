using Bogus;
using TeamHub.DAL.Models;

namespace TeamHub.Tests.Fakers
{
    public class CommentFaker : Faker<Comment>
    {
        public CommentFaker()
        {
            RuleFor(c => c.Id, f => f.IndexFaker);
            RuleFor(c => c.AuthorId, f => f.Random.Number());
            RuleFor(c => c.TasksId, f => f.Random.Number());
            RuleFor(c => c.Content, f => f.Lorem.Sentence());
            RuleFor(c => c.CreatedAt, f => f.Date.Past());
        }
    }
}
