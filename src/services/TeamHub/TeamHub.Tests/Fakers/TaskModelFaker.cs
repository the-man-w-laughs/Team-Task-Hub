using Bogus;
using Shared.Enums;
using TeamHub.DAL.Models;

namespace TeamHub.Tests.Fakers
{
    public class TaskModelFaker : Faker<TaskModel>
    {
        public TaskModelFaker()
        {
            RuleFor(t => t.Id, f => f.IndexFaker);
            RuleFor(t => t.AuthorTeamMemberId, f => f.Random.Number());
            RuleFor(t => t.ProjectId, f => f.Random.Number());
            RuleFor(t => t.PriorityId, f => f.PickRandom<TaskPriorityEnum>());
            RuleFor(t => t.Content, f => f.Lorem.Sentence());
            RuleFor(t => t.Deadline, f => f.Date.Future());
            RuleFor(t => t.IsCompleted, f => f.Random.SByte(0, 1));
            RuleFor(t => t.CreatedAt, f => f.Date.Past());
        }
    }
}
