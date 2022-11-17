using Bogus;
using ChapsDotNET.Data.Entities;

namespace ChapsDotNET.Business.Tests.Common
{
    public static class FakeReportsProvider
    {
        public static Faker<Report> FakeData { get; } =
            new Faker<Report>()
                .RuleFor(x => x.Name, f => f.Lorem.Word())
                .RuleFor(x => x.Description, f => f.Lorem.Sentence())
                .RuleFor(x => x.LongDescription, f => f.Lorem.Paragraph());
    }
}

