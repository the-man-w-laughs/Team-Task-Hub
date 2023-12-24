using Bogus;
using Shared.Enums;
using TeamHub.DAL.Models;

namespace TeamHub.Tests.Fakers
{
    public class TaskModelFaker : Faker<TaskModel>
    {
        public TaskModelFaker()
        {
            RuleFor(task => task.Id, faker => faker.IndexFaker);
            RuleFor(task => task.AuthorTeamMemberId, faker => faker.Random.Number());
            RuleFor(task => task.ProjectId, faker => faker.Random.Number());
            RuleFor(task => task.PriorityId, faker => faker.PickRandom<TaskPriorityEnum>());
            RuleFor(task => task.Content, faker => faker.Lorem.Sentence());
            RuleFor(task => task.Deadline, faker => faker.Date.Future());
            RuleFor(task => task.IsCompleted, faker => faker.Random.SByte(0, 1));
            RuleFor(task => task.CreatedAt, faker => faker.Date.Past());
        }
    }
}
