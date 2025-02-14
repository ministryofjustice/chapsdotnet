using Bogus;
using ChapsDotNET.Data.Entities;

namespace ChapsDotNET.Business.Tests.Common
{
	public class FakeMPsProvider
	{
		public static Faker<MP> FakeData { get; } = new Faker<MP>()
            .RuleFor(x => x.RtHon, f => f.Random.Bool())
            .RuleFor(x => x.SalutationID, f => f.Random.Number(1, 10))
            .RuleFor(x => x.Surname, f => f.Name.LastName())
            .RuleFor(x => x.FirstNames, f => f.Name.FirstName())
            .RuleFor(x => x.Email, (f, x) => f.Internet.Email(x.FirstNames, x.Surname))
            .RuleFor(x => x.AddressLine1, f => f.Address.BuildingNumber())
            .RuleFor(x => x.AddressLine2, f => f.Address.SecondaryAddress())
            .RuleFor(x => x.AddressLine3, f => f.Address.StreetName())
            .RuleFor(x => x.Town , f => f.Address.City())
            .RuleFor(x => x.County, f => f.Address.County())
            .RuleFor(x => x.Postcode, f => f.Address.ZipCode())
            .RuleFor(x => x.Suffix, f => f.Name.Suffix())
            .RuleFor(x => x.active, f => true);
	}
}
