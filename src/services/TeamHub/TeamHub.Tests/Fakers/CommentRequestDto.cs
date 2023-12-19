using Bogus;
using TeamHub.BLL.Dtos;

namespace TeamHub.Tests.Fakers
{
    public class CommentRequestDtoFaker : Faker<CommentRequestDto>
    {
        public CommentRequestDtoFaker()
        {
            RuleFor(commentRequest => commentRequest.Content, faker => faker.Lorem.Sentence());
        }
    }
}
