using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Business.Tests.Common;
using ChapsDotNET.Data.Entities;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ChapsDotNET.Business.Tests
{
    public class EmailTemplateComponentTests
    {
        [Fact(DisplayName = "Adding an Email Template with the AddEmailTemplate method returns the correct EmailTemplateId")]
        public async Task AddAnEmailTemplateToTheDatabase()
        {
            // Arrange
            var context = DataContextFactory.Create();
            var emailTemplateComponent = new EmailTemplateComponent(context);
            var emailTemplateModel = new Models.EmailTemplateModel
            {
                EmailTemplateID = 1,
                CorrespondenceTypeID = 1,
                StageID = 1,
                Chaser = false,
                Subject = "Test Subject",
                BodyText = "Test Body Text"
            };
            //Act
            var x = await emailTemplateComponent.AddEmailTemplateAsync(emailTemplateModel);
            var emailTemplateId = await emailTemplateComponent.AddEmailTemplateAsync(emailTemplateModel);
            //Assert
            emailTemplateId.Should().BeGreaterThan(0);
            context.EmailTemplates.First().Should().NotBeNull();


        }

        //[Fact(DisplayName = "Get a specific Email Template")]
        //public async Task GetASpecificEmailTemplateAsync()
        //{
        //    // Arrange
        //    var context = DataContextFactory.Create();

        //    await context.EmailTemplates.AddAsync(new EmailTemplate
        //    {
        //        EmailTemplateID = 1,
        //        CorrespondenceTypeID = 1,
        //        StageID = 1,
        //        Chaser = false,
        //        Subject = "Test Subject",
        //        BodyText = "Test Body Text"
        //    }
        //    );

        //    await context.EmailTemplates.AddAsync(new EmailTemplate
        //    {
        //        EmailTemplateID = 3,
        //        CorrespondenceTypeID = 3,
        //        StageID = 3,
        //        Chaser = false,
        //        Subject = "Test Subject 3",
        //        BodyText = "Test Body Text 3"
        //    }
        //    );

        //    await context.SaveChangesAsync();

        //    var EmailTemplateComponent = new EmailTemplateComponent(context);

        //    // Act
        //    var result = await EmailTemplateComponent.GetEmailTemplateAsync(3);

        //    // Assert
        //    result.Active.Should().BeFalse();
        //    result.CorrespondenceTypeID.Should().Be("3");
        //    result.StageID.Should().Be("3");
        //    result.Chaser.Should().Be(false);
        //    result.Subject.Should().Be("Test Subject 3");
        //    result.BodyText.Should().Be("Test Body Text 3");
        //}


    }
}