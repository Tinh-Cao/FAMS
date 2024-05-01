using Application.ViewModels.ResponseModels;
using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.API.Controllers;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.ViewModels.ScoreModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;


namespace WebAPI.Tests.Controllers
{
    public class ScoreControllerTests : SetupTest
    {
        private readonly ScoresController _scoreController;
        private readonly Fixture _fixture;

        public ScoreControllerTests()
        {
            _scoreController = new ScoresController(_scoreServiceMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetScoresByFilters_ShouldReturnCorrectData()
        {
            // Arrange
            var paginationParameter = _fixture.Create<PaginationParameter>();
            var scoreFilterModel = _fixture.Create<ScoreFilterModel>();
            var scores = _fixture.CreateMany<ScoreViewModel>(10).ToList();
            var expectedResult = new Pagination<ScoreViewModel>(scores, 10, 1, 1);
            _scoreServiceMock.Setup(x => x.GetScoresByFiltersAsync(paginationParameter, scoreFilterModel))
                               .ReturnsAsync(expectedResult);
            var httpContext = new DefaultHttpContext();
            var response = new Mock<HttpResponse>();
            var headers = new HeaderDictionary(); // Initialize headers
            headers.Add("X-Pagination", ""); // Add X-Pagination header
            response.SetupGet(r => r.Headers).Returns(headers); // Set the value for "X-Pagination"
            var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            var controllerContext = new ControllerContext(actionContext);
            // Act
            _scoreController.ControllerContext = controllerContext;
            var result = await _scoreController.GetScoresByFilters(paginationParameter, scoreFilterModel);
            // Assert
            _scoreServiceMock.Verify(x => x.GetScoresByFiltersAsync(paginationParameter, scoreFilterModel), Times.Once);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetScoreByStudent_ShouldReturnModel()
        {
            // Arrange
            var studentId = 1;
            var classId = 1;
            var scores = _fixture.Create<ScoreViewModel>();
            // Mock the behavior of the score service to return a successful result
            _scoreServiceMock.Setup(x => x.GetScoreByIdAsync(studentId, classId)).ReturnsAsync(scores);
            // Act
            var result = await _scoreController.GetScoresByStudentId(studentId, classId);
            // Assert
            var okObjectResult = (OkObjectResult)result;
            Assert.NotNull(okObjectResult);
            Assert.True(okObjectResult.StatusCode == 200);
            Assert.Equal(scores, okObjectResult.Value);
        }

        [Fact]
        public async Task AddScore_ShouldReturnSuccess()
        {
            // Arrange
            var scores = _fixture.Create<ScoreCreateModel>();
            // Mock the behavior of the score service to return a successful result
            _scoreServiceMock.Setup(x => x.AddScoreByFormAsync(scores)).ReturnsAsync(new ResponseModel { Status = true });
            // Act
            var result = await _scoreController.AddScoresByForm(scores);
            // Assert
            var okObjectResult = (OkObjectResult)result;
            Assert.NotNull(okObjectResult);
            Assert.True(okObjectResult.StatusCode == 200);
            Assert.True(((ResponseModel)okObjectResult.Value).Status);
        }

        [Fact]
        public async Task AddScore_ShouldReturnFailure()
        {
            // Arrange
            var scores = _fixture.Create<ScoreCreateModel>();
            // Mock the behavior of the score service to return a failure result
            _scoreServiceMock.Setup(x => x.AddScoreByFormAsync(scores)).ReturnsAsync(new ResponseModel { Status = false });
            // Act
            var result = await _scoreController.AddScoresByForm(scores);
            // Assert
            var badRequestObjectResult = (BadRequestObjectResult)result;
            Assert.NotNull(badRequestObjectResult);
            Assert.True(badRequestObjectResult.StatusCode == 400);
            Assert.False(((ResponseModel)badRequestObjectResult.Value).Status);
        }

        [Fact]
        public async Task ImportScores_ShouldReturnSuccess()
        {
            // Arrange
            var scores = _fixture.CreateMany<ScoreImportModel>(10).ToList();
            // Mock the behavior of the score service to return a successful result
            _scoreServiceMock.Setup(x => x.ScoreImportExcel(scores)).ReturnsAsync(new ScoreImportResponseModel { Status = true });
            // Act
            var result = await _scoreController.ImportScore(scores);
            // Assert
            var okObjectResult = (OkObjectResult)result;
            Assert.NotNull(okObjectResult);
            Assert.True(okObjectResult.StatusCode == 200);
            Assert.True(((ScoreImportResponseModel)okObjectResult.Value).Status);
        }

        [Fact]
        public async Task ImportAccounts_ShouldReturnFailure()
        {
            // Arrange
            var scores = _fixture.CreateMany<ScoreImportModel>(10).ToList();
            // Mock the behavior of the score service to return a failure result
            _scoreServiceMock.Setup(x => x.ScoreImportExcel(scores)).ReturnsAsync(new ScoreImportResponseModel { Status = false });
            // Act
            var result = await _scoreController.ImportScore(scores);
            // Assert
            var badRequestObjectResult = (BadRequestObjectResult)result;
            Assert.NotNull(badRequestObjectResult);
            Assert.True(badRequestObjectResult.StatusCode == 400);
            Assert.False(((ScoreImportResponseModel)badRequestObjectResult.Value).Status);
        }

        [Fact]
        public async Task UpdateScore_ShouldReturnSuccess()
        {
            // Arrange
            var studentId = 1;
            var classId = 1;
            var scores = _fixture.Create<ScoreUpdateModel>();
            // Mock the behavior of the score service to return a successful result
            _scoreServiceMock.Setup(x => x.UpdateScoreByStudentAsync(studentId, classId, scores)).ReturnsAsync(new ResponseModel { Status = true });
            // Act
            var result = await _scoreController.UpdateScoresByStudent(studentId, classId, scores);
            // Assert
            var okObjectResult = (OkObjectResult)result;
            Assert.NotNull(okObjectResult);
            Assert.True(okObjectResult.StatusCode == 200);
            Assert.True(((ResponseModel)okObjectResult.Value).Status);
        }

        [Fact]
        public async Task UpdateScore_ShouldReturnFailure()
        {
            // Arrange
            var studentId = 1;
            var classId = 1;
            var scores = _fixture.Create<ScoreUpdateModel>();
            // Mock the behavior of the score service to return a failure result
            _scoreServiceMock.Setup(x => x.UpdateScoreByStudentAsync(studentId, classId, scores)).ReturnsAsync(new ResponseModel { Status = false });
            // Act
            var result = await _scoreController.UpdateScoresByStudent(studentId, classId, scores);
            // Assert
            var badRequestObjectResult = (BadRequestObjectResult)result;
            Assert.NotNull(badRequestObjectResult);
            Assert.True(badRequestObjectResult.StatusCode == 400);
            Assert.False(((ResponseModel)badRequestObjectResult.Value).Status);
        }

        [Fact]
        public async Task UpdateScoreByClass_ShouldReturnSuccess()
        {
            //Arrange
            var scores = _fixture.CreateMany<ScoreCreateModel>(10).ToList();
            // Mock the behaviour of the score service to return a successful result
            _scoreServiceMock.Setup(x => x.UpdateScoreByClassAsync(scores)).ReturnsAsync(new ResponseModel { Status = true });
            // Act
            var result = await _scoreController.UpdateScoresByClass(scores);
            // Assert
            var okObjectResult = (OkObjectResult)result;
            Assert.NotNull(okObjectResult);
            Assert.True(okObjectResult.StatusCode == 200);
            Assert.True(((ResponseModel)okObjectResult.Value).Status);
        }

        [Fact]
        public async Task UpdateScoreByClass_ShouldReturnFailure()
        {
            // Arrange
            var scores = _fixture.CreateMany<ScoreCreateModel>(10).ToList();
            // Mock the behaviour of the score service to return a failure result
            _scoreServiceMock.Setup(x => x.UpdateScoreByClassAsync(scores)).ReturnsAsync(new ResponseModel { Status = false });
            // Act
            var result = await _scoreController.UpdateScoresByClass(scores);
            // Assert
            var badRequestObjectResult = (BadRequestObjectResult)result;
            Assert.NotNull(badRequestObjectResult);
            Assert.True(badRequestObjectResult.StatusCode == 400);
            Assert.False(((ResponseModel)badRequestObjectResult.Value).Status);
        }

        [Fact]
        public async Task DeleteScoreByStudent_ShouldReturnSuccess()
        {
            // Arrange
            var StudentId = 1;
            var ClassId = 1;
            // Mock the behaviour of the score service to return a successful result
            _scoreServiceMock.Setup(x => x.DeleteScoreAsync(StudentId, ClassId)).ReturnsAsync(new ResponseModel { Status = true });
            // Act
            var result = await _scoreController.DeleteScore(StudentId, ClassId);
            // Assert
            var okObjectResult = (OkObjectResult)result;
            Assert.NotNull(okObjectResult);
            Assert.True(okObjectResult.StatusCode == 200);
            Assert.True(((ResponseModel)okObjectResult.Value).Status);
        }

        [Fact]
        public async Task DeleteScoreByStudent_ShouldReturnFailure()
        {
            // Arrange
            var StudentId = 1;
            var ClassId = 1;
            // Mock the behaviour of the score service to return a failure result
            _scoreServiceMock.Setup(x => x.DeleteScoreAsync(StudentId, ClassId)).ReturnsAsync(new ResponseModel { Status = false });
            // Act
            var result = await _scoreController.DeleteScore(StudentId, ClassId);
            // Assert
            var badRequestObjectResult = (BadRequestObjectResult)result;
            Assert.NotNull(badRequestObjectResult);
            Assert.True(badRequestObjectResult.StatusCode == 400);
            Assert.False(((ResponseModel)badRequestObjectResult.Value).Status);
        }

        [Fact]
        public async Task DeleteScoresByClass_Success()
        {
            // Arrange
            var studentIds = new List<int> { 1, 2, 3 };
            var classId = 1;

            // Mock the behaviour of the score service to return a failure result
            _scoreServiceMock.Setup(x => x.DeleteScoresByClassAsync(studentIds, classId)).ReturnsAsync(new ResponseModel { Status = true });
            // Act
            var result = await _scoreController.DeleteScoresByClass(studentIds, classId);
            // Assert
            var okObjectResult = (OkObjectResult)result;
            Assert.NotNull(okObjectResult);
            Assert.True(okObjectResult.StatusCode == 200);
            Assert.True(((ResponseModel)okObjectResult.Value).Status);
        }

        [Fact]
        public async Task DeleteScoresByClass_Failure()
        {
            // Arrange
            var studentIds = new List<int> { 1, 2, 3 };
            var classId = 1;

            // Mock the behaviour of the score service to return a failure result
            _scoreServiceMock.Setup(x => x.DeleteScoresByClassAsync(studentIds, classId)).ReturnsAsync(new ResponseModel { Status = false }) ;
            // Act
            var result = await _scoreController.DeleteScoresByClass(studentIds, classId);
            // Assert
            var badRequestObjectResult = (BadRequestObjectResult)result;
            Assert.NotNull(badRequestObjectResult);
            Assert.True(badRequestObjectResult.StatusCode == 400);
            Assert.False(((ResponseModel)badRequestObjectResult.Value).Status);
        }
    }
}
