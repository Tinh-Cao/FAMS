using Application.ViewModels.ResponseModels;
using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.API.Controllers;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.ViewModels.EmailModel;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace WebAPI.Tests.Controllers
{
    public class EmailTemplateControllerTest : SetupTest
    {
        private readonly Fixture _fixture;
        private readonly EmailTemplateController _emailTemplatecontroller;

        public EmailTemplateControllerTest()
        {
            _fixture = new Fixture();
            _emailTemplatecontroller = new EmailTemplateController(_emailTemplateServiceMock.Object);

        }
        [Fact]
        public async Task GetAllEmailTempalte_ShouldReturnSuccess()
        {
            //arrange

            var emailTemplates = _fixture.Build<EmailTemplate>().With(c => c.EmailSends, new List<EmailSend>()).CreateMany();
            //Mock the behavior of the student service to return a successful result
            _emailTemplateServiceMock.Setup(x => x.GetAllEmailTemplateAsync()).ReturnsAsync(emailTemplates.ToList());
            //act
            var result = await _emailTemplatecontroller.GetAlllEmailTemplate();
            //Assert
            var okObjectResult = (OkObjectResult)result;
            okObjectResult.Value.Should().BeEquivalentTo(emailTemplates);
            Assert.True(okObjectResult.StatusCode == 200);
            Assert.Equal(emailTemplates, okObjectResult.Value);
        }
        [Fact]
        public async Task GetEmailTemplateByIdAsync_ReturnsOkObjectResult()
        {
            // Arrange
            var emailTemplate = new EmailTemplate();
            _emailTemplateServiceMock.Setup(x => x.GetAllEmailTemplateByIdAsync(1)).ReturnsAsync(emailTemplate);

            // Act
            var result = await _emailTemplatecontroller.GetEmailTemplateByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            result.Should().BeOfType<OkObjectResult>();
            var okObjectResult = (OkObjectResult)result;
            Assert.True(okObjectResult.StatusCode == 200);

        }

        [Fact]
        public async Task GetEmailTemplateByIdAsync_ReturnsNotFoundResult()
        {
            // Arrange
            EmailTemplate emailTemplate = null;
            _emailTemplateServiceMock.Setup(x => x.GetAllEmailTemplateByIdAsync(1)).ReturnsAsync(emailTemplate);

            // Act
            var result = await _emailTemplatecontroller.GetEmailTemplateByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            result.Should().BeOfType<NotFoundResult>();
            var notFoundResult = (NotFoundResult)result;
            Assert.True(notFoundResult.StatusCode == 404);
        }
        [Fact]
        public async Task DeleteEmailTemplate_ReturnsBadRequestResult()
        {
            // Arrange
            int id = 1;
            _emailTemplateServiceMock.Setup(x => x.DeleteEmailTemplateAsync(id)).Throws(new Exception());

            // Act
            var result = await _emailTemplatecontroller.DeleteEmailTemplate(id);

            // Assert
            Assert.NotNull(result);
            result.Should().BeOfType<BadRequestResult>();
            var badRequestResult = (BadRequestResult)result;
            Assert.True(badRequestResult.StatusCode == 400);
        }
        [Fact]
        public async Task DeleteEmailTemplate_ShouldReturnSuccess()
        {
            // Arrange
            var templateId = 1;
            _emailTemplateServiceMock.Setup(x => x.DeleteEmailTemplateAsync(templateId))
                                      .ReturnsAsync(new  ResponseModel { Status = true, Message = " deleted successfully" });

            // Act
            var result = await _emailTemplatecontroller.DeleteEmailTemplate(templateId);

            // Assert
            Assert.NotNull(result);
            result.Should().BeOfType<OkObjectResult>();
            var okObjectResult = (OkObjectResult)result;
            Assert.Equal(200, okObjectResult.StatusCode);
        }
        [Fact]
        public async Task DeleteEmailTemplate_ShouldReturnNotFound()
        {
            // Arrange
            var templateId = 1;
            _emailTemplateServiceMock.Setup(x => x.DeleteEmailTemplateAsync(templateId))
                                      .ReturnsAsync(new ResponseModel { Status = false, Message = " does not exist" });

            // Act
            var result = await _emailTemplatecontroller.DeleteEmailTemplate(templateId);

            // Assert
            Assert.NotNull(result);
            result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        // Unit test for PostEmailTemplate action
        [Fact]
        public async Task PostEmailTemplate_ShouldReturnSuccess()
        {
            // Arrange
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var emailTemplateModel = _fixture.Create<EmailTemplateModel>();
            var emailTemplate = _fixture.Create<EmailTemplate>();
            _emailTemplateServiceMock.Setup(x => x.AddEmailTemplateAsync(emailTemplateModel)).ReturnsAsync(emailTemplate);

            // Act
            var result = await _emailTemplatecontroller.PostEmailTemplate(emailTemplateModel);

            // Assert
            Assert.NotNull(result);
            result.Should().BeOfType<ActionResult<EmailTemplateResponseModel>>();
            var actionResult = (ActionResult<EmailTemplateResponseModel>)result;
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var okObjectResult = (OkObjectResult)actionResult.Result;
            Assert.True(okObjectResult.StatusCode == 200);
            Assert.Equal(emailTemplate, okObjectResult.Value);
        }

        // Unit test for UpdateEmailTemplate action
        [Fact]
        public async Task UpdateEmailTemplate_ShouldReturnSuccess()
        {
            // Arrange
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var id = 1;
            var emailTemplateModel = _fixture.Create<EmailTemplateModel>();
            var updatedEmailTemplate = _fixture.Create<EmailTemplate>();
            _emailTemplateServiceMock.Setup(x => x.UpdateEmailTemplateAsync(id, emailTemplateModel)).ReturnsAsync(updatedEmailTemplate);

            // Act
            var result = await _emailTemplatecontroller.UpdateEmailTemplate(id, emailTemplateModel);

            // Assert
            Assert.NotNull(result);
            result.Should().BeOfType<ActionResult<EmailTemplateResponseModel>>();
            var actionResult = (ActionResult<EmailTemplateResponseModel>)result;
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var okObjectResult = (OkObjectResult)actionResult.Result;
            Assert.True(okObjectResult.StatusCode == 200);
            Assert.Equal(updatedEmailTemplate, okObjectResult.Value);
        }

        // Unit test for BanEmailTemplate action
        [Fact]
        public async Task BanEmailTemplate_ShouldReturnSuccess()
        {
            // Arrange
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var id = 1;
            var emailTemplate = _fixture.Create<EmailTemplate>();
            _emailTemplateServiceMock.Setup(x => x.BanEmailTemplateAsync(id)).ReturnsAsync(emailTemplate);

            // Act
            var result = await _emailTemplatecontroller.BanEmailTemplate(id);

            // Assert
            Assert.NotNull(result);
            result.Should().BeOfType<ActionResult<EmailTemplateResponseModel>>();
            var actionResult = (ActionResult<EmailTemplateResponseModel>)result;
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var okObjectResult = (OkObjectResult)actionResult.Result;
            Assert.True(okObjectResult.StatusCode == 200);
            Assert.Equal(emailTemplate, okObjectResult.Value);
        }

        // Unit test for UnBanEmailTemplate action
        [Fact]
        public async Task UnBanEmailTemplate_ShouldReturnSuccess()
        {
            // Arrange
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var id = 1;
            var emailTemplate = _fixture.Create<EmailTemplate>();
            _emailTemplateServiceMock.Setup(x => x.UnBanEmailTemplateAsync(id)).ReturnsAsync(emailTemplate);

            // Act
            var result = await _emailTemplatecontroller.UnBanEmailTemplate(id);

            // Assert
            Assert.NotNull(result);
            result.Should().BeOfType<ActionResult<EmailTemplateResponseModel>>();
            var actionResult = (ActionResult<EmailTemplateResponseModel>)result;
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var okObjectResult = (OkObjectResult)actionResult.Result;
            Assert.True(okObjectResult.StatusCode == 200);
            Assert.Equal(emailTemplate, okObjectResult.Value);
        }
    }
}
