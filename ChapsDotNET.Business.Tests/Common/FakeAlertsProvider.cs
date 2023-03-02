﻿using Bogus;
using ChapsDotNET.Data.Entities;

namespace ChapsDotNET.Business.Tests.Common
{
    public static class FakeAlertsProvider
    {
        public static Faker<Alert> FakeData { get; } =
            new Faker<Alert>()
                .RuleFor(x => x.Message, f => f.Lorem.Word())
                .RuleFor(x => x.live, f => f.Random.Bool())
                .RuleFor(x => x.deactivated, f => f.Date.Past())
                .RuleFor(x => x.deactivatedBy, f => f.Name.FirstName());
    }
}