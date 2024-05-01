using Application.ViewModels.ResponseModels;
using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.API.Controllers;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.ViewModels.StudentModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Build.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Controllers
{
    public class ClassControllerTest : SetupTest
    {
        private readonly ClassController _classController;
        private readonly Fixture _fixture;
        public ClassControllerTest() 
        {
            _classController = new ClassController(_classServiceMock.Object);
            _fixture = new Fixture();
        }


        [Fact]
        public async Task CreateClass_ShouldReturnCorrectData()
        {
            //arrange
            var mockModelRequest = _fixture.Build<CreateClassModel>().Create();
            var responseDataModel = _fixture.Build<ResponseDataModel<ClassItemModel>>().Create();
            

            //Mock the behavior of the student service to return a successful result
            _classServiceMock.Setup(x => x.CreateClassAsync(mockModelRequest))
                .ReturnsAsync(new ResponseDataModel<ClassItemModel> { Status = true });
            //act
            var result =await _classController.CreateClass(mockModelRequest);
            //Assert
            _classServiceMock.Verify(x => x.CreateClassAsync(mockModelRequest), Times.Once);
            Assert.NotNull(result);
            result.Should().BeOfType<OkObjectResult>();
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult?.Value.Should().BeOfType<ResponseDataModel<ClassItemModel>>();

        }

        [Fact]
        public async Task GetClassesByFilters_ShouldReturnCorrectData()
        {
            // Arrange
            var paginationParameter = new PaginationParameter();
            var classesFilterModel = new ClassesFilterModel();
            var classes = _fixture.CreateMany<ClassItemModel>(10).ToList();
            var expectedResult = new Pagination<ClassItemModel>(classes, 10, 1, 1);

            _classServiceMock.Setup(x => x.GetClassesByFiltersAsync(paginationParameter,classesFilterModel))
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
            _classController.ControllerContext = controllerContext;
            var result = await _classController.GetClassesByFilters(paginationParameter, classesFilterModel);
            // Assert
            _classServiceMock.Verify(x => x.GetClassesByFiltersAsync(paginationParameter, classesFilterModel), Times.Once);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetAllByAccountID_ShouldReturnSuccessWithCorrectData()
        {
            // Arrange
            var mockRequest = new ClassesGetByAccountIdModel
            {
                AdminId = 1
            };

            _classServiceMock.Setup(x => x.GetClassesByAccountIdAsync(mockRequest)).ReturnsAsync(new List<ClassItemModel>());
            //act
            var result = await _classController.GetAllByAccountId(mockRequest);
            // Assert
            _classServiceMock.Verify(x => x.GetClassesByAccountIdAsync(mockRequest), Times.Once());// Check api calls
            Assert.NotNull(result);
            result.Should().BeOfType<OkObjectResult>(); //Success
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.True(okObjectResult.StatusCode == 200);
            var data = Assert.IsType<ResponseDataModel<IEnumerable<ClassItemModel>>>(okObjectResult.Value);
            Assert.IsType<List<ClassItemModel>>(data?.Data);

            // Verifying if the returned data is the class item details
        }


        [Fact]
        public async Task GetClassDetails_ShouldReturnSuccessWithCorrectData()
        {
            // Arrange
            int mockClassId = 11;
            var mockResponse = _fixture.Build<ResponseDataModel<ClassItemModel>>().Create();

            _classServiceMock.Setup(x => x.GetClassDetailsAsync(mockClassId)).ReturnsAsync(mockResponse);
            //act
            var result = await _classController.GetClassDetails(mockClassId);
            // Assert
            _classServiceMock.Verify(x => x.GetClassDetailsAsync(mockClassId), Times.Once());// Check api calls
            Assert.NotNull(result);
            result.Should().BeOfType<OkObjectResult>(); //Success
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.True(okObjectResult.StatusCode == 200);
            okObjectResult?.Value.Should().BeEquivalentTo(mockResponse);
            var data = (ResponseDataModel<ClassItemModel>)okObjectResult.Value;
            Assert.IsType<ClassItemModel>(data?.Data);

            // Verifying if the returned data is the class item details
        }

        [Fact]
        public async Task GetClassDetails_NonExistedClass_ShouldReturnNotfoundWithCorrectData()
        {
            // Arrange
            int mockClassId = 100;
            var mockResponse = _fixture.Build<ResponseDataModel<ClassItemModel>>().Create();

            _classServiceMock.Setup(x => x.GetClassDetailsAsync(mockClassId)).ReturnsAsync(new ResponseDataModel<ClassItemModel> { Status=false});
            //act
            var result = await _classController.GetClassDetails(mockClassId);
            // Assert
            _classServiceMock.Verify(x => x.GetClassDetailsAsync(mockClassId), Times.Once());// Check api calls
            Assert.NotNull(result);
            var okObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.True(okObjectResult.StatusCode == 404);
         //   okObjectResult?.Value.Should().Be(mockResponse);
            var data = (ResponseDataModel<ClassItemModel>)okObjectResult.Value;
            data?.Data.Should().BeNull();

            // Verifying if the returned data is the class item details
        }

        [Fact]
        public async Task AddStudentToClass_ShouldReturnCorrectData()
        {
            //arrange
            var mockModelRequest = _fixture.Build<StudentsClassModel>().Create();
            var responseDataModel = _fixture.Build<ResponseDataModel<AddStudentIntoClassResponseModel>>().Create();
            //Mock the behavior of the student service to return a successful result
            _classServiceMock.Setup(s => s.AddStudentToClass(mockModelRequest.studentIdList, mockModelRequest.classId)).ReturnsAsync(new ResponseDataModel<AddStudentIntoClassResponseModel> { Status=true});
            //act
            var result = await _classController.AddStudentToClass(mockModelRequest);
            //Assert
            _classServiceMock.Verify(x => x.AddStudentToClass(mockModelRequest.studentIdList, mockModelRequest.classId), Times.Once);
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True(okResult.StatusCode == 200);
        }

        [Fact]
        public async Task DeleteStudentFromClass_ShouldReturnCorrectData()
        {
            //arrange
            var mockModelRequest = _fixture.Build<StudentsClassModel>().Create();
            //Mock the behavior of the student service to return a successful result
            _classServiceMock.Setup(s => s.DeleteStudentFromClass(mockModelRequest.studentIdList, mockModelRequest.classId)).ReturnsAsync(new ResponseDataModel<ResponseModel> { Status = true });
            //act
            var result = await _classController.DeleteStudentFromClass(mockModelRequest);
            //Assert
            _classServiceMock.Verify(x => x.DeleteStudentFromClass(mockModelRequest.studentIdList, mockModelRequest.classId), Times.Once);
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True(okResult.StatusCode == 200);
        }

        [Fact]
        public async Task UpdateClass_ShouldReturnCorrectData()
        {
            //arrange
            var mockModelRequest = _fixture.Build<UpdateClassModel>().Create();
            var mockResponseModel = new ResponseModel { Status = true };
            //Mock the behavior of the student service to return a successful result
            _classServiceMock.Setup(s => s.UpdateClass(mockModelRequest, 1)).ReturnsAsync(mockResponseModel);
            //act
            var result = await _classController.UpdateClass(mockModelRequest,1);
            //Assert
            _classServiceMock.Verify(x => x.UpdateClass(mockModelRequest, 1), Times.Once);
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True(okResult.StatusCode == 200);
        }

    }
}
