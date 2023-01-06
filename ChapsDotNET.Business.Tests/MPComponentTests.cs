using Bogus;
using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Business.Tests.Common;
using ChapsDotNET.Data.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using NSubstitute;

namespace ChapsDotNET.Business.Tests
{
    public class MPComponentTests
    {
        [Fact(DisplayName = "Get a list of MPs when GetMPsAsync method is called")]
        public async Task GetAListOfMPsWhenGetMPsAsyncMethodIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.MPs.AddAsync(new MP
                {
                    MPID = 1,
                    RtHon = false,
                    SalutationID = 3,
                    Surname = "Janeway",
                    FirstNames = "Katherine",
                    Email = "kathrine.janeway@starfleet.com",
                    AddressLine1 = "StarFleet", 
                    AddressLine2 = "Head Quarters",
                    AddressLine3 = "",
                    Town = "San Francisco",
                    County = "",
                    Postcode = "XX33 Q45",
                    Suffix = "PHD",
                    active = true
                }
            );
            await context.SaveChangesAsync();

            var mockSalutationComponent = Substitute.For<ISalutationComponent>();
            var mpComponent = new MPComponent(context, mockSalutationComponent);


            // Act
            var result = await mpComponent.GetMPsAsync(new MPRequestModel());

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(1);
            result.Results?.First().MPId.Should().Be(1);
            result.Results?.First().RtHon.Should().BeFalse();
            result.Results?.First().SalutationId.Should().Be(3);
            result.Results?.First().Surname.Should().Be("Janeway");
            result.Results?.First().FirstNames.Should().Be("Katherine");
            result.Results?.First().Email.Should().Be("kathrine.janeway@starfleet.com");
            result.Results?.First().AddressLine1.Should().Be("StarFleet");
            result.Results?.First().AddressLine2.Should().Be("Head Quarters");
            result.Results?.First().AddressLine3.Should().Be("");
            result.Results?.First().Town.Should().Be("San Francisco");
            result.Results?.First().County.Should().Be("");
            result.Results?.First().Postcode.Should().Be("XX33 Q45");
            result.Results?.First().Suffix.Should().Be("PHD");
            result.Results?.First().Active.Should().BeTrue();
        }

        [Fact(DisplayName = "Get a list of only active MPs when GetMPsAsync method is called without true in the parameter")]
        public async Task GetAListOfOnlyActiveMPsWhenGetMPsAsyncMethodIsCalledWithoutTrueInTheParameter()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.MPs.AddAsync(new MP
                {
                    MPID = 1,
                    RtHon = false,
                    SalutationID = 3,
                    Surname = "Janeway",
                    FirstNames = "Katherine",
                    Email = "kathrine.janeway@starfleet.com",
                    AddressLine1 = "StarFleet HQ", 
                    AddressLine2 = "",
                    AddressLine3 = "",
                    Town = "San Francisco",
                    County = "",
                    Postcode = "XX33 Q45",
                    Suffix = "PHD",
                    active = true
                }
            );
            await context.MPs.AddAsync(new MP
                {
                    MPID = 2,
                    RtHon = true,
                    SalutationID = 28,
                    Surname = "Picard",
                    FirstNames = "Jean Luc",
                    Email = "j.picard@chateau-picard.com",
                    AddressLine1 = "Chateau Picard", 
                    AddressLine2 = "Le Rue Dragon",
                    AddressLine3 = "",
                    Town = "Lyon",
                    County = "Aquitane",
                    Postcode = "NC17 C01",
                    Suffix = "",
                    active = false
                }
            );
            await context.MPs.AddAsync(new MP
                {
                    MPID = 3,
                    RtHon = true,
                    SalutationID = 17,
                    Surname = "Troy",
                    FirstNames = "Deanna",
                    Email = "d.troyd@starfleet.com",
                    AddressLine1 = "Dun Roaming", 
                    AddressLine2 = "",
                    AddressLine3 = "",
                    Town = "New York",
                    County = "",
                    Postcode = "NC17 C02",
                    Suffix = " Msc",
                    active = true
                }
            );
            await context.MPs.AddAsync(new MP
                {
                    MPID = 4,
                    RtHon = false,
                    SalutationID = 3,
                    Surname = "Yar",
                    FirstNames = "Tasha",
                    Email = "t.yar@starfleet.com",
                    AddressLine1 = "55", 
                    AddressLine2 = "Towie way",
                    AddressLine3 = "",
                    Town = "Loughton",
                    County = "Essex",
                    Postcode = "NC17 C02",
                    Suffix = "",
                    active = false
                }
            );

            await context.SaveChangesAsync();

            var mockSalutationComponent = Substitute.For<ISalutationComponent>();
            var mpComponent = new MPComponent(context, mockSalutationComponent);

            // Act
            var result = await mpComponent.GetMPsAsync(new MPRequestModel());

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(2);
            result.Results?.All(x => x.Active).Should().BeTrue();
        }

        [Fact(DisplayName = "Get a list of all MPs when GetMPsAsync method is called with Show Active in the parameter")]
        public async Task GetAListOfActiveMPsWhenGetMPsAsyncIsCalled()
        {
            // Arrange
            var context = DataContextFactory.Create();
            await context.MPs.AddAsync(new MP
                {
                    MPID = 1,
                    RtHon = false,
                    SalutationID = 3,
                    Surname = "Janeway",
                    FirstNames = "Katherine",
                    Email = "kathrine.janeway@starfleet.com",
                    AddressLine1 = "StarFleet HQ", 
                    AddressLine2 = "",
                    AddressLine3 = "",
                    Town = "San Francisco",
                    County = "",
                    Postcode = "XX33 Q45",
                    Suffix = "PHD",
                    active = true
                }
            );
            await context.MPs.AddAsync(new MP
                {
                    MPID = 2,
                    RtHon = true,
                    SalutationID = 28,
                    Surname = "Picard",
                    FirstNames = "Jean Luc",
                    Email = "j.picard@chateau-picard.com",
                    AddressLine1 = "Chateau Picard", 
                    AddressLine2 = "Le Rue Dragon",
                    AddressLine3 = "",
                    Town = "Lyon",
                    County = "Aquitane",
                    Postcode = "NC17 C01",
                    Suffix = "",
                    active = false
                }
            );
            await context.MPs.AddAsync(new MP
                {
                    MPID = 3,
                    RtHon = true,
                    SalutationID = 17,
                    Surname = "Troy",
                    FirstNames = "Deanna",
                    Email = "d.troyd@starfleet.com",
                    AddressLine1 = "Dun Roaming", 
                    AddressLine2 = "",
                    AddressLine3 = "",
                    Town = "New York",
                    County = "",
                    Postcode = "NC17 C02",
                    Suffix = " Msc",
                    active = true
                }
            );
            await context.MPs.AddAsync(new MP
                {
                    MPID = 4,
                    RtHon = false,
                    SalutationID = 3,
                    Surname = "Yar",
                    FirstNames = "Tasha",
                    Email = "t.yar@starfleet.com",
                    AddressLine1 = "55", 
                    AddressLine2 = "Towie way",
                    AddressLine3 = "",
                    Town = "Loughton",
                    County = "Essex",
                    Postcode = "NC17 C02",
                    Suffix = "",
                    active = false
                }
            );

            await context.SaveChangesAsync();

            var mockSalutationComponent = Substitute.For<ISalutationComponent>();
            var mpComponent = new MPComponent(context, mockSalutationComponent);

            // Act
            var result = await mpComponent.GetMPsAsync(new MPRequestModel
            {
                activeFilter = false
            });

            // Assert
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(4);
        }

        [Fact(DisplayName = "Calling GetSalutationsAsync method returns paged results by default")]
        public async Task CallingGetSalutationsAsyncMethodReturnsPagedResultsByDefault()
        {
            // Arrange
            var context = DataContextFactory.Create();
            var fakeMPs = FakeMPsProvider.FakeData
                .Generate(50)
                .ToList();
            await context.MPs.AddRangeAsync(fakeMPs);
            await context.SaveChangesAsync();
            var mockSalutationComponent = Substitute.For<ISalutationComponent>();
            var mpComponent = new MPComponent(context, mockSalutationComponent);

            // Act
            var result = await mpComponent.GetMPsAsync(new MPRequestModel
            {
                activeFilter = false
            });

            // Assert
            result.PageSize.Should().Be(10);
            result.PageCount.Should().Be(5);
            result.RowCount.Should().Be(50);
            result.CurrentPage.Should().Be(1);
            result.Results.Should().NotBeNull();
            result.Results.Should().HaveCount(10);
        }

        [Fact(DisplayName = "Get a specific MP")]
        public async Task GetASpecificMPAsync()
        {
            // Arrange
            var context = DataContextFactory.Create();

            await context.MPs.AddAsync(new MP
                {
                    MPID = 22,
                    RtHon = true,
                    SalutationID = 28,
                    Surname = "Picard",
                    FirstNames = "Jean Luc",
                    Email = "j.picard@chateau-picard.com",
                    AddressLine1 = "Chateau Picard", 
                    AddressLine2 = "Le Rue Dragon",
                    AddressLine3 = "",
                    Town = "Lyon",
                    County = "Aquitane",
                    Postcode = "NC17 C01",
                    Suffix = "",
                    active = false
                }
            );

            await context.MPs.AddAsync(new MP
                {
                    MPID = 44,
                    RtHon = false,
                    SalutationID = 3,
                    Surname = "Yar",
                    FirstNames = "Tasha",
                    Email = "t.yar@starfleet.com",
                    AddressLine1 = "55", 
                    AddressLine2 = "Towie Way",
                    AddressLine3 = "",
                    Town = "Loughton",
                    County = "Essex",
                    Postcode = "NC17 C02",
                    Suffix = "",
                    active = false
                }
            );

            await context.SaveChangesAsync();

            var mockSalutationComponent = Substitute.For<ISalutationComponent>();
            var mpComponent = new MPComponent(context, mockSalutationComponent);

            // Act
            var result = await mpComponent.GetMPAsync(44);

            // Assert
            result.Active.Should().BeFalse();
            result.AddressLine1.Should().Be("55");
            result.AddressLine2.Should().Be("Towie Way");
            result.AddressLine3.Should().Be("");
            result.County.Should().Be("Essex");
            result.Email.Should().Be("t.yar@starfleet.com");
            result.FirstNames.Should().Be("Tasha");
            result.MPId.Should().Be(44);
            result.Postcode.Should().Be("NC17 C02");
            result.RtHon.Should().BeFalse();
            result.SalutationId.Should().Be(3);
            result.Suffix.Should().Be("");
            result.Surname.Should().Be("Yar");
            result.Town.Should().Be("Loughton");
            result.Should().NotBeNull();
        }

        [Fact(DisplayName = "Test response to passing a non existent salutation id")]
        public async Task TestResponseToPassingANonExistentSalutationIdAsync()
        {
            // Arrange
            var context = DataContextFactory.Create();

            await context.MPs.AddAsync(new MP
                {
                    MPID = 22,
                    RtHon = true,
                    SalutationID = 28,
                    Surname = "Picard",
                    FirstNames = "Jean Luc",
                    Email = "j.picard@chateau-picard.com",
                    AddressLine1 = "Chateau Picard", 
                    AddressLine2 = "Le Rue Dragon",
                    AddressLine3 = "",
                    Town = "Lyon",
                    County = "Aquitane",
                    Postcode = "NC17 C01",
                    Suffix = "",
                    active = false
                }
            );

            await context.MPs.AddAsync(new MP
                {
                    MPID = 44,
                    RtHon = false,
                    SalutationID = 3,
                    Surname = "Yar",
                    FirstNames = "Tasha",
                    Email = "t.yar@starfleet.com",
                    AddressLine1 = "55", 
                    AddressLine2 = "Towie Way",
                    AddressLine3 = "",
                    Town = "Loughton",
                    County = "Essex",
                    Postcode = "NC17 C02",
                    Suffix = "",
                    active = false
                }
            );

            await context.SaveChangesAsync();

            var mockSalutationComponent = Substitute.For<ISalutationComponent>();
            var mpComponent = new MPComponent(context, mockSalutationComponent);

            // Act
            var result = await mpComponent.GetMPAsync(53);

            // Assert
            result.RtHon.Should().BeFalse();
            result.SalutationId.Should().BeNull();
            result.Surname.Should().BeEmpty();
            result.FirstNames.Should().BeNull();
            result.Email.Should().BeNull();
            result.AddressLine1.Should().BeNull();
            result.AddressLine2.Should().BeNull();
            result.AddressLine3.Should().BeNull();
            result.Town.Should().BeNull();
            result.County.Should().BeNull();
            result.Postcode.Should().BeNull();
            result.Suffix.Should().BeNull();
        }

        [Fact(DisplayName = "Adding an MP with the AddMP method returns the correct MPID")]
        public async Task AddAnMPToTheDatabase()
        {
            // Arrange
            var context = DataContextFactory.Create();
            var mockSalutationComponent = Substitute.For<ISalutationComponent>();
            var mpComponent = new MPComponent(context, mockSalutationComponent);
            var newrecord = new Models.MPModel
            {
                MPId = 8,
                RtHon = true,
                SalutationId = 14,
                Surname = "Crusher",
                FirstNames = "Beverly",
                Email = "beverly.crusher@starfleet.com",
                AddressLine1 = "23", 
                AddressLine2 = "The Glebe",
                AddressLine3 = "High Street",
                Town = "Austin",
                County = "Texas",
                Postcode = "AB1 2CD",
                Suffix = "Msc",
                Active = true
            };

            // Act
            var result = await mpComponent.AddMPAsync(newrecord);

            // Assert
            result.Should().NotBe(0);
            context.MPs.First().SalutationID.Should().Be(14);
            context.MPs.First().RtHon.Should().BeTrue();
            context.MPs.First().Surname.Should().Be("Crusher");
            context.MPs.First().FirstNames.Should().Be("Beverly");
            context.MPs.First().Email.Should().Be("beverly.crusher@starfleet.com");
            context.MPs.First().AddressLine1.Should().Be("23");
            context.MPs.First().AddressLine2.Should().Be("The Glebe");
            context.MPs.First().AddressLine3.Should().Be("High Street");
            context.MPs.First().Town.Should().Be("Austin");
            context.MPs.First().County.Should().Be("Texas");
            context.MPs.First().Postcode.Should().Be("AB1 2CD");
            context.MPs.First().Suffix.Should().Be("Msc");
            context.MPs.First().active.Should().BeTrue();
        }

        [Fact(DisplayName = "Adding an MP without a surname should throw a ArgumentNullException")]
        public async Task AddingAnMPWithoutASurnameShouldThrowAArgumentNullException()
        {
            // Arrange
            var context = DataContextFactory.Create();
            var mockSalutationComponent = Substitute.For<ISalutationComponent>();
            var mpComponent = new MPComponent(context, mockSalutationComponent);
            var newrecord = new Models.MPModel
            {
                MPId = 8,
                RtHon = true,
                SalutationId = 14,
                Surname = "",
                FirstNames = "Beverly",
                Email = "beverly.crusher@starfleet.com",
                AddressLine1 = "23",
                AddressLine2 = "The Glebe",
                AddressLine3 = "High Street",
                Town = "Austin",
                County = "Texas",
                Postcode = "AB1 2CD",
                Suffix = "Msc",
                Active = false
            };

            //Act
            Func<Task> act = async () => { await mpComponent.AddMPAsync(newrecord); };

            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact(DisplayName = "Updating an MPs active status by calling UpdateSalutationActiveStatus")]
        public async Task UpdatingnMPsActiveStatusByCallingUpdateSalutationActiveStatus()
        {
            // Arrange
            var context = DataContextFactory.Create();

            await context.MPs.AddAsync(new MP
            {
                MPID = 2,
                RtHon = true,
                SalutationID = 28,
                Surname = "Picard",
                FirstNames = "Jean Luc",
                Email = "j.picard@chateau-picard.com",
                AddressLine1 = "Chateau Picard", 
                AddressLine2 = "Le Rue Dragon",
                AddressLine3 = "",
                Town = "Lyon",
                County = "Aquitane",
                Postcode = "NC17 C01",
                Suffix = "",
                active = true
            });

            await context.SaveChangesAsync();

            var mockSalutationComponent = Substitute.For<ISalutationComponent>();
            var mpComponent = new MPComponent(context, mockSalutationComponent);

            // Act
            await mpComponent.UpdateMPAsync(new Models.MPModel
            {
                MPId = 2,
                RtHon = true,
                SalutationId = 28,
                Surname = "Picard",
                FirstNames = "Jean Luc",
                Email = "j.picard@chateau-picard.com",
                AddressLine1 = "Chateau Picard", 
                AddressLine2 = "Le Rue Dragon",
                AddressLine3 = "",
                Town = "Lyon",
                County = "Aquitane",
                Postcode = "NC17 C01",
                Suffix = "",
                Active = false
            });

            // Assert
            context.MPs.First().RtHon.Should().BeTrue();
            context.MPs.First().SalutationID.Should().Be(28);
            context.MPs.First().Surname.Should().Be("Picard");
            context.MPs.First().FirstNames.Should().Be("Jean Luc");
            context.MPs.First().Email.Should().Be("j.picard@chateau-picard.com");
            context.MPs.First().AddressLine1.Should().Be("Chateau Picard");
            context.MPs.First().AddressLine2.Should().Be("Le Rue Dragon");
            context.MPs.First().AddressLine3.Should().Be("");
            context.MPs.First().Town.Should().Be("Lyon");
            context.MPs.First().County.Should().Be("Aquitane");
            context.MPs.First().Postcode.Should().Be("NC17 C01");
            context.MPs.First().Suffix.Should().Be("");
            context.MPs.First().active.Should().BeFalse();
        }

        [Fact(DisplayName = "Updating an MPs active status by calling the UpdateSalutationAsync with no ID")]
        public async Task UpdateActiveStatusOnAnMPWithNoIDShouldThrowAnException()
        {
            // Arrange
            var context = DataContextFactory.Create();

            await context.MPs.AddAsync(new MP
            {
                MPID = 1,
                RtHon = false,
                SalutationID = 3,
                Surname = "Janeway",
                FirstNames = "Katherine",
                Email = "kathrine.janeway@starfleet.com",
                AddressLine1 = "StarFleet", 
                AddressLine2 = "Head Quarters",
                AddressLine3 = "",
                Town = "San Francisco",
                County = "",
                Postcode = "XX33 Q45",
                Suffix = "PHD",
                active = true
            });

            await context.SaveChangesAsync();
            var mockSalutationComponent = Substitute.For<ISalutationComponent>();
            var mpComponent = new MPComponent(context, mockSalutationComponent);

            // Act
            Func<Task> act = async () => { await mpComponent.UpdateMPAsync(new Models.MPModel()); };

            //Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact(DisplayName = "Updating an MP with no surname should throw an ArgumentNullException")]
        public async Task UpdateAnMPWithNoSurnameShouldThrowAnException()
        {
            // Arrange
            var context = DataContextFactory.Create();

            await context.MPs.AddAsync(new MP
            {
                MPID = 3,
                RtHon = true,
                SalutationID = 17,
                Surname = "Troy",
                FirstNames = "Deanna",
                Email = "d.troyd@starfleet.com",
                AddressLine1 = "Dun Roaming", 
                AddressLine2 = "",
                AddressLine3 = "",
                Town = "New York",
                County = "",
                Postcode = "NC17 C02",
                Suffix = " Msc",
                active = true
            });

            await context.SaveChangesAsync();
            var mockSalutationComponent = Substitute.For<ISalutationComponent>();
            var mpComponent = new MPComponent(context, mockSalutationComponent);

            // Act
            Func<Task> act = async () =>
            {
                await mpComponent.UpdateMPAsync(new Models.MPModel
                {
                    MPId = 3,
                    RtHon = true,
                    SalutationId = 17,
                    Surname = string.Empty,
                    FirstNames = "Deanna",
                    Email = "d.troyd@starfleet.com",
                    AddressLine1 = "Dun Roaming", 
                    AddressLine2 = "",
                    AddressLine3 = "",
                    Town = "New York",
                    County = "",
                    Postcode = "NC17 C02",
                    Suffix = " Msc",
                    Active = true
                });
            };

            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
