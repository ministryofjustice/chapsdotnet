using Bogus;
using ChapsDotNET.Data.Entities;

namespace ChapsDotNET.Business.Tests.Common
{
    public static class FakeSalutationsProvider
    {
        public static Faker<Salutation> FakeData { get; } =
            new Faker<Salutation>()
                .RuleFor(x => x.Detail, f => f.Name.Prefix())
                .RuleFor(x => x.active, f => f.Random.Bool())
                .RuleFor(x => x.deactivated, f => f.Date.Past())
                .RuleFor(x => x.deactivatedBy, f => f.Name.FirstName());
    }
}
