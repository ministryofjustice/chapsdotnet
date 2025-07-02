using ChapsDotNET.Areas.Admin.Controllers;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ChapsDotNET.Tests.Areas
{
	public class MPControllerTests
	{
        // TODO: fix test to incorporate filter references
        //[Fact]
        //public async Task WhenMPIndexPageIsCalledAListOfMPsShouldBeReturned()
        //{
        //    //Arrange
        //    var mockMPComponent = Substitute.For<IMPComponent>();
        //    var mockSalutationComponent = Substitute.For<ISalutationComponent>();
        //    mockMPComponent.GetMPsAsync(Arg.Any<MPRequestModel>()).Returns(
        //        new PagedResult<List<MPModel>>
        //        {
        //            Results = new List<MPModel>()
        //            {
        //                new MPModel
        //                {
        //                    MPId = 1,
        //                    RtHon = false,
        //                    SalutationId = 3,
        //                    Surname = "Janeway",
        //                    FirstNames = "Katherine",
        //                    Email = "kathrine.janeway@starfleet.com",
        //                    AddressLine1 = "StarFleet HQ",
        //                    AddressLine2 = "",
        //                    AddressLine3 = "",
        //                    Town = "San Francisco",
        //                    County = "",
        //                    Postcode = "XX33 Q45",
        //                    Suffix = "PHD",
        //                    Active = true
        //                },

        //                new MPModel
        //                {
        //                    MPId = 2,
        //                    RtHon = true,
        //                    SalutationId = 28,
        //                    Surname = "Picard",
        //                    FirstNames = "Jean Luc",
        //                    Email = "j.picard@chateau-picard.com",
        //                    AddressLine1 = "Chateau Picard",
        //                    AddressLine2 = "Le Rue Dragon",
        //                    AddressLine3 = "",
        //                    Town = "Lyon",
        //                    County = "Aquitane",
        //                    Postcode = "NC17 C01",
        //                    Suffix = "",
        //                    Active = false
        //                }
        //            }
        //        }
        //    );
        //    var controller = new MPsController(mockMPComponent, mockSalutationComponent);

        //    // Act
        //    var result = await controller.Index() as ViewResult;
        //    var resultCount = result?.Model as List<MPModel>;

        //    // Assert
        //    await mockMPComponent.Received().GetMPsAsync(Arg.Any<MPRequestModel>());
        //    result.Should().NotBe(null);
        //    result.Should().BeOfType<ViewResult>();
        //    resultCount?.Count.Should().Be(2);
        //}

        [Fact]
        public async Task WhenCreateMethodIsCalledTheCreateViewIsReturned()
        {
            //Arrange
            var mockMPComponent = Substitute.For<IMPComponent>();
            var mockSalutationComponent = Substitute.For<ISalutationComponent>();
            var mockUserComponent = Substitute.For<IUserComponent>();
            var mockSalutation = mockSalutationComponent.GetSalutationsAsync(Arg.Any<SalutationRequestModel>()).Returns(
                new PagedResult<List<SalutationModel>>
                {
                    Results = new List<SalutationModel>()
                    {
                        new SalutationModel
                        {
                            Detail = "Lord",
                            SalutationId = 1,
                            Active = true
                        },
                    }
                }
            );

            var controller = new MPsController(mockMPComponent, mockSalutationComponent, mockUserComponent);

            //Act
            var result = await controller.Create();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task WhenCreateMethodIsCalledTheAddMPsAsyncMethodIsCalled()
        {
            //Arrange
            var mockMPComponent = Substitute.For<IMPComponent>();
            var mockSalutationComponent = Substitute.For<ISalutationComponent>();
            var mockUserComponent = Substitute.For<IUserComponent>();
            var controller = new MPsController(mockMPComponent, mockSalutationComponent, mockUserComponent);
            var mpViewModel = new MPViewModel()
            {
                MPId = 1,
                RtHon = false,
                SalutationId = 3,
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
                Active = true
            };

            mockMPComponent.AddMPAsync(Arg.Any<MPModel>()).Returns(1);
            mockMPComponent.GetMPAsync(1).Returns(new MPModel
            {
                MPId = 1,
                RtHon = true,
                SalutationId = 1,
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
                Active = true
            });

            var httpContext = new DefaultHttpContext();
            var mockTempData = Substitute.For<ITempDataProvider>();
            var tempData = new TempDataDictionary(httpContext, mockTempData)
            {
                ["alertContent"] = "YourValue"
            };
            controller.TempData = tempData;


            //Act
            var result = await controller.Create(mpViewModel);

            //Assert
            await mockMPComponent.Received().AddMPAsync(Arg.Any<MPModel>());
            result.Should().NotBe(null);
        }

        [Fact]
        public async Task WhenEditMethodIsCalledTheEditViewIsReturned()
        {
            //Arrange
            var mockMPComponent = Substitute.For<IMPComponent>();
            var mockSalutationComponent = Substitute.For<ISalutationComponent>();
            var mockUserComponent = Substitute.For<IUserComponent>();
            var mockSalutation = mockSalutationComponent.GetSalutationsAsync(Arg.Any<SalutationRequestModel>()).Returns(
                new PagedResult<List<SalutationModel>>
                {
                    Results = new List<SalutationModel>()
                    {
                        new SalutationModel
                        {
                            Detail = "Lord",
                            SalutationId = 1,
                            Active = true
                        },
                    }
                }
            );

            var mockMpModel = new MPModel
            {
                MPId = 1,
                RtHon = true,
                SalutationId = 1,
                Surname = "Picard",
                FirstNames = "Jean Luc",
                Email = "j.picard@chateau-picard.co.uk",
                AddressLine1 = "Chateau Picard",
                AddressLine2 = "Le Rue Dragon",
                AddressLine3 = "",
                Town = "Lyon",
                County = "Aquitane",
                Postcode = "NC17 C01",
                Suffix = "",
                Active = true,
                DisplayFullName = "RtHon Lord Jean Luc Picard"
            };

            mockMPComponent.GetMPAsync(1).Returns(mockMpModel);

            var controller = new MPsController(mockMPComponent, mockSalutationComponent, mockUserComponent);

            mockMPComponent.GetMPAsync(1).Returns(new MPModel
            {
                MPId = 1,
                RtHon = true,
                SalutationId = 1,
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
                Active = true
            });

            var httpContext = new DefaultHttpContext();
            var mockTempData = Substitute.For<ITempDataProvider>();
            var tempData = new TempDataDictionary(httpContext, mockTempData)
            {
                ["alertContent"] = "YourValue"
            };
            controller.TempData = tempData;

            //Act
            var result = await controller.Edit(1);

            //Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task WhenTheEditScreenSaveButtonIsClickedUpdateMPAsyncMethodIsCalled()
        {
            //Arrange
            var mockMPComponent = Substitute.For<IMPComponent>();
            var mockSalutationComponent = Substitute.For<ISalutationComponent>();
            var mockUserComponent = Substitute.For<IUserComponent>();

            var mockMPModel = new MPModel
            {
                MPId = 3,
                RtHon = false,
                SalutationId = 17,
                Surname = "Troy",
                FirstNames = "Deanna",
                Email = "d.troyd@starfleet.co.uk",
                AddressLine1 = "Dun Roaming",
                AddressLine2 = "",
                AddressLine3 = "",
                Town = "New York",
                County = "",
                Postcode = "NC17 C02",
                Suffix = " Msc",
                Active = true
            };
            mockMPComponent.GetMPAsync(3).Returns(mockMPModel);

            var controller = new MPsController(mockMPComponent, mockSalutationComponent, mockUserComponent);

            var httpContext = new DefaultHttpContext();
            var mockTempData = Substitute.For<ITempDataProvider>();
            var tempData = new TempDataDictionary(httpContext, mockTempData)
            {
                ["alertContent"] = "YourValue"
            };
            controller.TempData = tempData;


            //Act
            var result = await controller.Edit(new MPViewModel
            {
                MPId = 3,
                RtHon = false,
                SalutationId = 17,
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
                Active = true
            });

            //Assert
            await mockMPComponent.Received().UpdateMPAsync(Arg.Any<MPModel>());
            result.Should().BeOfType<RedirectToActionResult>();
        }
	}
}
