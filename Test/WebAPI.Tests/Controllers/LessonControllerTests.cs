using Application.ViewModels.ResponseModels;
using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.API.Controllers;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.LessonModels;
using Microsoft.AspNetCore.Routing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.CodeAnalysis;
using Microsoft.SqlServer.Server;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Controllers
{
    public class LessonControllerTests : SetupTest
    {
        private readonly LessonController _lessconController;
        private readonly Fixture _fixture;
        public LessonControllerTests()
        {
            _lessconController = new LessonController(_lessonServiceMock.Object);
            _fixture = new Fixture();
        }
        [Fact]
        public async Task CreateLesson_ShouldReturnOk_WhenLessonAddedSuccessfully()
        {
            // Arrange
            var lessonViewModel = _fixture.Create<CreateLessonModel>();
            _lessonServiceMock.Setup(x => x.CreateLessonAsync(lessonViewModel))
                              .ReturnsAsync(new ResponseModel { Status = true, Message = "Added Lesson Successfully" });
            // Act
            var result = await _lessconController.CreateLesson(lessonViewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var response = Assert.IsType<ResponseModel>(okResult.Value);
            Assert.True(response.Status);
            Assert.Equal("Added Lesson Successfully", response.Message);
        }
        [Fact]
        public async Task CreateLesson_ShouldReturnBadRequest_WhenErrorAddingLesson()
        {
            // Arrange
            var lessViewModel = _fixture.Create<CreateLessonModel>();
            _lessonServiceMock.Setup(x => x.CreateLessonAsync(lessViewModel))
                              .ReturnsAsync(new ResponseModel { Status = false, Message = "Error added" });
            // Act
            var result = await _lessconController.CreateLesson(lessViewModel);


            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            var response = Assert.IsType<ResponseModel>(badRequestResult.Value);
            Assert.False(response.Status);
            Assert.Equal("Error added", response.Message);
        }
        [Fact]
        public async Task GetAllLessonAsync_ShouldReturnListOfLessonDetailsModel()
        {
            // Arrange
            var mockLessonDetails = _fixture.Build<Lesson>().Without(a => a.Module).Without(a => a.Documents).CreateMany(3).ToList();
            _lessonServiceMock.Setup(x => x.GetAllLessonAsync()).ReturnsAsync(mockLessonDetails);
            // Act
            var result = await _lessconController.GetAllLesson();

            // Assert          
            var okResult = Assert.IsType<OkObjectResult>(result);
            var lessonDetails = (List<Lesson>)okResult.Value;
            Assert.Equal(mockLessonDetails.Count, lessonDetails.Count);
        }
        [Fact]
        public async Task GetAllLessonAsync_ShouldReturnNotFound_WhenNoLessonFound()
        {
            // Arrange
            _lessonServiceMock.Setup(x => x.GetAllLessonAsync()).ReturnsAsync((List<Lesson>)null);
            // Act
            var result = await _lessconController.GetAllLesson();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
        [Fact]
        public async Task GetLessonById_ShouldReturnLessonDetails_WhenLessonExists()
        {
            // Arrange 
            var lessonId = 1;
            var expectedLesson = _fixture.Build<Lesson>().Without(a => a.Module).Without(a => a.Documents).Create();
            _lessonServiceMock.Setup(x => x.GetLessonByIdAsync(lessonId)).ReturnsAsync(expectedLesson);
            // Act
            var result = await _lessconController.GetLessonById(lessonId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var LessonDetails = Assert.IsType<Lesson>(okResult.Value);
            Assert.Equal(expectedLesson, LessonDetails);
        }
        [Fact]
        public async Task GetLessonById_ShouldReturnNotFound_WhenLessonNotExists()
        {
            // Arrange 
            var lessonId = 1;
            _lessonServiceMock.Setup(x => x.GetLessonByIdAsync(lessonId)).ReturnsAsync((Lesson)null);
            // Act
            var result = await _lessconController.GetLessonById(lessonId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
        [Fact]
        public async Task UpdateLesson_ShouldReturnOk_WhenLessonUpdateSuccesfully()
        {
            // Arrange
            var lessonId = 1;
            var updateLessonModel = _fixture.Create<UpdateLessonModel>();
            _lessonServiceMock.Setup(x => x.UpdateLessonAsync(lessonId, updateLessonModel))
                              .ReturnsAsync(new ResponseModel { Status = true, Message = "Lesson update successfully!" });
            // Act
            var result = await _lessconController.UpdateLessonAsync(lessonId, updateLessonModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var response = Assert.IsType<ResponseModel>(okResult.Value);
            Assert.True(response.Status);
            Assert.Equal("Lesson update successfully!", response.Message);
        }
        [Fact]
        public async Task UpdateLesson_ShouldReturnNotFound_WhenLessonNotFound()
        {
            // Arrange
            var lessonId = 1;
            var updateLessonModel = _fixture.Create<UpdateLessonModel>();
            _lessonServiceMock.Setup(x => x.UpdateLessonAsync(lessonId, updateLessonModel))
                              .ReturnsAsync(new ResponseModel { Status = false, Message = "Lesson does not exists" });
            // Act
            var result = await _lessconController.UpdateLessonAsync(lessonId, updateLessonModel);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            var response = Assert.IsType<ResponseModel>(notFoundResult.Value);
            Assert.False(response.Status);
            Assert.Equal("Lesson does not exists", response.Message);
        }
        [Fact]
        public async Task DeleteLesson_ShouldReturnSuccess()
        {
            // Arrange
            var lessonId = 1;
            _lessonServiceMock.Setup(x => x.DeleteLessonAsync(lessonId))
                              .ReturnsAsync(new ResponseModel { Status = true, Message = "Lesson deleted successfully" });
            // Act
            var result = await _lessconController.DeleteLessonAsync(lessonId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public async Task DeleteLesson_ShouldReturnNotFound_WhenLessonNotFound()
        {
            // Arrange
            var lessonId = 1;
            _lessonServiceMock.Setup(x => x.DeleteLessonAsync(lessonId))
                              .ReturnsAsync(new ResponseModel { Status = false, Message = "Lesson does not exists" });
            // Act
            var result = await _lessconController.DeleteLessonAsync(lessonId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
        [Fact]
        public async Task GetLessonFilters_ShouldReturnCorrectData()
        {
            // Arrange
            var paginationParameter = _fixture.Create<PaginationParameter>();
            var lessonFilterModel = _fixture.Create<LessonFilterModel>();
            var lessons = _fixture.CreateMany<LessonDetailsModel>(10).ToList();
            var expectedResult = new Pagination<LessonDetailsModel>(lessons, 10, 1, 1);
            _lessonServiceMock.Setup(x => x.GetLessonByFilterAsync(paginationParameter, lessonFilterModel))
                                .ReturnsAsync(expectedResult);
            var httpContext = new DefaultHttpContext();
            var response = new Mock<HttpResponse>();
            var headers = new HeaderDictionary();
            headers.Add("X-Pagination", "");
            response.SetupGet(r => r.Headers).Returns(headers);
            var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            var controllerContext = new ControllerContext(actionContext);
            // Act
            _lessconController.ControllerContext = controllerContext;
            var result = await _lessconController.GetLessonByFilter(paginationParameter, lessonFilterModel);
            // Assert
            _lessonServiceMock.Verify(x => x.GetLessonByFilterAsync(paginationParameter, lessonFilterModel), Times.Once);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedResult);
        }
    }
}
