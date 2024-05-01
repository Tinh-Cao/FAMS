using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.API.Controllers;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.EmailSendsModels;
using FAMS_GROUP2.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Controllers
{
    public class EmailSendControllerTests : SetupTest
    {
        private readonly Mock<IEmailSendService> _emailSendServiceMock;
        private readonly EmailSendController _emailSendController;

        public EmailSendControllerTests()
        {
            _emailSendServiceMock = new Mock<IEmailSendService>();
            _emailSendController = new EmailSendController(_emailSendServiceMock.Object);
        }

        [Fact]
        public async Task GetEmailSendsByFilter_ShouldReturnOkResultWithPaginationData()
        {
            // Arrange
            var paginationParameter = new PaginationParameter
            {
                PageIndex = 1,
                PageSize = 10 // Assuming page size is 10 for the test
            };
            var emailSendsFilterModule = new EmailSendsFilterModule
            {
                StartDate = DateTime.Now.AddDays(-7), // Example start date
                EndDate = DateTime.Now // Example end date
            };
            var emailSendPaginationResult = GetSampleEmailSendPaginationResult();

            _emailSendServiceMock.Setup(x => x.GetAllEmailSendFilterBySendDateAsync(paginationParameter, emailSendsFilterModule))
                .ReturnsAsync(emailSendPaginationResult);

            // Setup ControllerContext
            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            _emailSendController.ControllerContext = controllerContext;

            // Act
            var result = await _emailSendController.GetEmailSendsByFilter(paginationParameter, emailSendsFilterModule);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var emailSendResult = okResult.Value.Should().BeAssignableTo<Pagination<EmailSend>>().Subject;

            emailSendResult.TotalCount.Should().Be(emailSendPaginationResult.TotalCount);
            emailSendResult.PageSize.Should().Be(emailSendPaginationResult.PageSize);
            emailSendResult.CurrentPage.Should().Be(emailSendPaginationResult.CurrentPage);
            emailSendResult.TotalPages.Should().Be(emailSendPaginationResult.TotalPages);
            emailSendResult.HasNext.Should().Be(emailSendPaginationResult.HasNext);
            emailSendResult.HasPrevious.Should().Be(emailSendPaginationResult.HasPrevious);
        }

        private Pagination<EmailSend> GetSampleEmailSendPaginationResult()
        {
            // Generate sample data for pagination result
            var emailSends = Enumerable.Range(1, 10).Select(i => new EmailSend
            {
                SenderId = i,
                TemplateId = i,
                SendDate = DateTime.Now.AddDays(-i),
                Subject = $"Subject {i}",
                Content = $"Content {i}"
            }).ToList();

            return new Pagination<EmailSend>(emailSends, count: 100, pageNumber: 1, pageSize: 10);
        }
    }
}
