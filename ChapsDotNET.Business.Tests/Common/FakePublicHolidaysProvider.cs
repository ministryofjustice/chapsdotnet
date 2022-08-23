using System;
using Bogus;  
using ChapsDotNET.Data.Entities;

namespace ChapsDotNET.Business.Tests.Common
{
    public class FakePublicHolidaysProvider
    {
        public static Faker<PublicHoliday> FakeData { get; } =
            new Faker<PublicHoliday>()
                .RuleFor(x => x.Date, f => f.Date.Between(new DateTime(2022, 01, 01), new DateTime(2023, 01, 01)))
                .RuleFor(x => x.Description, f => f.Lorem.Word());
                //.RuleFor(x => x.PublicHolidayID, f => f.UniqueIndex);
    }
}
