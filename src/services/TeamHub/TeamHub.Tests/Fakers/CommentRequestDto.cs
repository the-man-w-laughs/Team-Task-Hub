using Bogus;
using TeamHub.BLL.Dtos;

namespace TeamHub.Tests.Fakers
{
    public class CommentRequestDtoFaker : Faker<CommentRequestDto>
    {
        public CommentRequestDtoFaker()
        {
            RuleFor(c => c.Content, f => f.Lorem.Sentence());
        }
    }
}
