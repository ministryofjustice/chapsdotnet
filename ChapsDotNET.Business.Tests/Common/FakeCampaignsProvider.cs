using Bogus;
using ChapsDotNET.Data.Entities;

namespace ChapsDotNET.Business.Tests.Common
{
    public static class FakeCampaignsProvider
    {
        public static Faker<Campaign> FakeData { get; } =
            new Faker<Campaign>()
                .RuleFor(x => x.Detail, f => f.Lorem.Word())
                .RuleFor(x => x.active, f => f.Random.Bool())
                .RuleFor(x => x.deactivated, f => f.Date.Past())
                .RuleFor(x => x.deactivatedBy, f => f.Name.FirstName());
    }
}
