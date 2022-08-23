using System;
using Bogus;
using ChapsDotNET.Data.Entities;

namespace ChapsDotNET.Business.Tests.Common
{
	public static class FakeMoJMinistersProvider
	{
		public static Faker<MoJMinister> FakeData { get; } = new Faker<MoJMinister>()
            .RuleFor(x => x.prefix, f => f.Name.Prefix())
            .RuleFor(x => x.Name, f => f.Name.FullName())
            .RuleFor(x => x.suffix, f => f.Name.Suffix())
            .RuleFor(x => x.active, f => f.Random.Bool());
	}
}
