using AutoFixture;
using AutoMapper;
using Domain.Tests;
using FAMS_GROUP2.API.Controllers;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.ViewModels.StudentModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Controllers
{
    public class StudentControllerTests : SetupTest
    {
        private readonly StudentsController _studentsController;
        private readonly Fixture _fixture;

        public StudentControllerTests()
        {
            _fixture = new Fixture();
            _studentsController = new StudentsController( _studentServiceMock.Object);

        }

        [Fact]

        public async Task ImportStudents_ShouldReturnSuccess()
        {
            //arrange
            var students = _fixture.CreateMany<StudentImportModel>(10).ToList();
            //Mock the behavior of the student service to return a successful result
            _studentServiceMock.Setup(x => x.AddRangeStudent(students)).ReturnsAsync(new ImportStudentResponseModel { AddedStudents = students , Status=true});
            //act
            var result = await _studentsController.ImportRangeStudent(students);
            //Assert
            var okObjectResult =result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.True(okObjectResult.StatusCode== 200);
            Assert.IsType<ImportStudentResponseModel>(okObjectResult.Value);
            Assert.True(((ImportStudentResponseModel)okObjectResult.Value).Status);
            Assert.NotNull(((ImportStudentResponseModel)okObjectResult.Value).AddedStudents);

        }

        [Fact]
        public async Task ImportStudentsApi_ShouldReturnCorrectData()
        {
            //arrange
            var mockModelRequest = _fixture.CreateMany<StudentImportModel>(10).ToList();
            var mockModelResponse = _fixture.Build<ImportStudentResponseModel>().Create();
            //Mock the behavior of the student service to return a successful result
            _studentServiceMock.Setup(x => x.AddRangeStudent(mockModelRequest)).ReturnsAsync(mockModelResponse);
            //act
            var result = await _studentsController.ImportRangeStudent(mockModelRequest);
            //Assert

            //_studentServiceMock.Verify(x => x.AddRangeStudent(mockModelRequest), Times.Once());
            _studentServiceMock.Verify(x => x.AddRangeStudent(It.Is<List<StudentImportModel>>(x => x.Equals(mockModelRequest))), Times.Once());
            Assert.NotNull(result);
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeEquivalentTo(mockModelResponse);
        }

        [Fact]
        public async Task GetStudentDetails_ShouldReturnSuccessWithCorrectData()
        {
            // Arrange
            var mockResponse = _fixture.Build<StudentDetailsModel>().Create();

            _studentServiceMock.Setup(x => x.GetStudentDetailsAsync(16)).ReturnsAsync(mockResponse);
            //act
            var result = await _studentsController.GetAccountDetails(16);
            // Assert
            _studentServiceMock.Verify(x => x.GetStudentDetailsAsync(16), Times.Once());// Check api calls
            Assert.NotNull(result);
            result.Should().BeOfType<OkObjectResult>(); //Success
            var okObjectResult = (OkObjectResult)result;
            Assert.True(okObjectResult.StatusCode == 200);
            okObjectResult?.Value.Should().BeEquivalentTo(mockResponse);

            // Verifying if the returned data is the student details
        }


        [Fact]
        public async Task GetStudentDetails_ShouldReturnNotfound()
        {
            // Arrange
            var mockResponse = _fixture.Build<StudentDetailsModel>().Create();

            _studentServiceMock.Setup(x => x.GetStudentDetailsAsync(100)).ReturnsAsync((StudentDetailsModel)null);
            //act
            var result = await _studentsController.GetAccountDetails(100);
            // Assert
            _studentServiceMock.Verify(x => x.GetStudentDetailsAsync(100), Times.Once());// Check api calls
            Assert.NotNull(result);
            result.Should().BeOfType<NotFoundObjectResult>(); //Success
            var NotFoundResult = (NotFoundObjectResult)result;
            Assert.True(NotFoundResult.StatusCode == 404);
            Assert.Equal(404, NotFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteStudent_ShouldReturnSuccess()
        {
            // Arrange
            List<int> deleteIds = new List<int> { 15 };
            _studentServiceMock.Setup(x => x.DeleteStudentAsync(deleteIds)).ReturnsAsync(new List<StudentDetailsModel>());
            //act
            var result = await _studentsController.DeleteStudent(deleteIds);
            // Assert
            _studentServiceMock.Verify(x => x.DeleteStudentAsync(deleteIds), Times.Once());// Check api calls
            Assert.NotNull(result);
            result.Should().BeOfType<OkObjectResult>(); //Success
            var okObjectResult = (OkObjectResult)result;
            Assert.True(okObjectResult.StatusCode == 200);
        }

        [Fact]
        public async Task DeleteStudent_ShouldReturnUnsuccessNotfound()
        {
            // Arrange
            List<int> deleteIds = new List<int> {10000};
            _studentServiceMock.Setup(x => x.DeleteStudentAsync(deleteIds)).ReturnsAsync((List<StudentDetailsModel>)null);
            //act
            var result = await _studentsController.DeleteStudent(deleteIds);
            // Assert
            _studentServiceMock.Verify(x => x.DeleteStudentAsync(deleteIds), Times.Once());// Check api calls
            Assert.NotNull(result);
            result.Should().BeOfType<NotFoundObjectResult>(); //Success
            var NotFoundResult = (NotFoundObjectResult)result;
            Assert.True(NotFoundResult.StatusCode == 404);
            Assert.Equal(404, NotFoundResult.StatusCode);
        }
        [Fact]
        public async Task GetAccountByFilters_ShouldReturnCorrectData()
        {
            // Arrange
            var paginationParameter = new PaginationParameter();
            var studentFilterModel = new StudentFilterModel();
            var students = _fixture.CreateMany<StudentDetailsModel>(10).ToList();
            var expectedResult = new Pagination<StudentDetailsModel>(students, 10, 1, 1);

            _studentServiceMock.Setup(x => x.GetStudentsByFiltersAsync(paginationParameter, studentFilterModel))
                               .ReturnsAsync(expectedResult);
            //config for header
            var httpContext = new DefaultHttpContext();
            var response = new Mock<HttpResponse>();
            var headers = new HeaderDictionary  
            {
                { "X-Pagination", "" } // Initialize headers and Add X-Pagination header
            };
            response.SetupGet(r => r.Headers).Returns(headers); // Set the value for "X-Pagination"
            var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            var controllerContext = new ControllerContext(actionContext);
            // Act
            _studentsController.ControllerContext = controllerContext;
            var result = await _studentsController.GetStudentByFilter(paginationParameter, studentFilterModel);
            // Assert
            _studentServiceMock.Verify(x => x.GetStudentsByFiltersAsync(paginationParameter, studentFilterModel), Times.Once);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedResult);
        }

    }

}
