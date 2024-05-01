using Application.ViewModels.ResponseModels;
using AutoFixture;
using Azure;
using Domain.Tests;
using FAMS_GROUP2.API.Controllers;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.AssignmentModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using Newtonsoft.Json;

namespace WebAPI.Tests.Controllers;

public class AssignmentControllerTests : SetupTest
{
    private readonly AssignmentController _assignmentController;
    private readonly Fixture _fixture;

    public AssignmentControllerTests()
    {
        _assignmentController = new AssignmentController(_assignmentServiceMock.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetAsmsByFiltersAsync_ShouldReturnCorrectData()
    {
        // Arrange
        var paginationParameter = _fixture.Create<PaginationParameter>();
        var asmFilterModel = _fixture.Create<AssignmentFilterModel>();
        var assignments = _fixture.CreateMany<AssignmentViewModel>(5).ToList();
        var expectedResult = new Pagination<AssignmentViewModel>(assignments, 5, 1, 1);
        var metadata = new
        {
            expectedResult.TotalCount,
            expectedResult.PageSize,
            expectedResult.CurrentPage,
            expectedResult.TotalPages,
            expectedResult.HasNext,
            expectedResult.HasPrevious
        };
        _assignmentServiceMock.Setup(x => x.GetAsmsByFiltersAsync(paginationParameter, asmFilterModel))
            .ReturnsAsync(expectedResult);

        var httpContext = new DefaultHttpContext();
        var response = new Mock<HttpResponse>();
        var headers = new HeaderDictionary();

        headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
        response.SetupGet(x => x.Headers).Returns(headers);

        
        var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
        var controllerContext = new ControllerContext(actionContext);

        // Act
        _assignmentController.ControllerContext = controllerContext;
        var result = await _assignmentController.GetAsmsByFiltersAsync(paginationParameter, asmFilterModel);

        // Assert
        _assignmentServiceMock.Verify(x => x.GetAsmsByFiltersAsync(paginationParameter, asmFilterModel), Times.Once);
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject; 
        _assignmentController.ControllerContext.HttpContext.Response.Headers.Should().ContainKey("X-Pagination")
            .WhoseValue.Should().BeEquivalentTo(JsonConvert.SerializeObject(metadata));
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedResult);
        
    }
    
     [Fact]
    public async Task GetAsmsByFiltersAsync_ShouldReturnBadRequest()
    {
        // Arrange
        var paginationParameter = _fixture.Create<PaginationParameter>();
        var asmFilterModel = _fixture.Create<AssignmentFilterModel>();
        var assignments = _fixture.CreateMany<AssignmentViewModel>(5).ToList();
        var expectedResult = new Pagination<AssignmentViewModel>(assignments, 5, 1, 1);
        var metadata = new
        {
            expectedResult.TotalCount,
            expectedResult.PageSize,
            expectedResult.CurrentPage,
            expectedResult.TotalPages,
            expectedResult.HasNext,
            expectedResult.HasPrevious
        };
        _assignmentServiceMock.Setup(x => x.GetAsmsByFiltersAsync(paginationParameter, asmFilterModel))
            .ThrowsAsync(new Exception());

        var httpContext = new DefaultHttpContext();
        var response = new Mock<HttpResponse>();
        var headers = new HeaderDictionary();

        headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
        response.SetupGet(x => x.Headers).Returns(headers);

        
        var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
        var controllerContext = new ControllerContext(actionContext);

        // Act
        _assignmentController.ControllerContext = controllerContext;
        var result = await _assignmentController.GetAsmsByFiltersAsync(paginationParameter, asmFilterModel);

        // Assert
        _assignmentServiceMock.Verify(x => x.GetAsmsByFiltersAsync(paginationParameter, asmFilterModel), Times.Once);
        var okResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        _assignmentController.ControllerContext.HttpContext.Response.Headers.Should().NotContainKey("X-Pagination");
        okResult.StatusCode.Should().Be(400);
        okResult.Value.Should().BeOfType<ResponseModel>();
        
    }

    [Fact]
    public async Task CreateAsmByExcelAsync_ShoulReturnOkResult()
    {
        // Arrange
        var moduleId = 1;
        var asms = new List<AssignmentImportModel>();
        var expectedResult = _fixture.Create<AssignmentResponseModel>();
        _assignmentServiceMock.Setup(x => x.CreateAsmByExcelAsync(moduleId, asms)).ReturnsAsync(expectedResult);
        
        // Act
        var result = await _assignmentController.CreateAsmByExcelAsync(moduleId, asms);
        
        // Assert
        _assignmentServiceMock.Verify(x => x.CreateAsmByExcelAsync(moduleId, asms), Times.Once);
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact]
    public async Task CreateAsmByExcelAsync_ShoulReturnBadRequestResult()
    {
        // Arrange
        var moduleId = 1;
        var asms = new List<AssignmentImportModel>();
        _assignmentServiceMock.Setup(x => x.CreateAsmByExcelAsync(moduleId, asms)).ThrowsAsync(new Exception());
        
        // Act
        var result = await _assignmentController.CreateAsmByExcelAsync(moduleId, asms);
        
        // Assert
        _assignmentServiceMock.Verify(x => x.CreateAsmByExcelAsync(moduleId, asms), Times.Once);
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(400);
        badRequestResult.Value.Should().BeOfType<ResponseModel>();
    }
    
    [Fact]
    public async Task UpdateAsmAsync_ShoulReturnOkResult()
    {
        // Arrange
        var moduleId = 1;
        var asm = new AssignmentImportModel();
        var expectedResult = _fixture.Create<ResponseModel>();
        _assignmentServiceMock.Setup(x => x.UpdateAsmAsync(moduleId, asm)).ReturnsAsync(expectedResult);
        
        // Act
        var result = await _assignmentController.UpdateAsmAsync(moduleId, asm);
        
        // Assert
        _assignmentServiceMock.Verify(x => x.UpdateAsmAsync(moduleId, asm), Times.Once);
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeOfType<ResponseModel>();
    }
    
    [Fact]
    public async Task UpdateAsmAsync_ShoulReturnBadRequestResult()
    {
        // Arrange
        var moduleId = 1;
        var asm = new AssignmentImportModel();
        _assignmentServiceMock.Setup(x => x.UpdateAsmAsync(moduleId, asm)).ThrowsAsync(new Exception());
        
        // Act
        var result = await _assignmentController.UpdateAsmAsync(moduleId, asm);
        
        // Assert
        _assignmentServiceMock.Verify(x => x.UpdateAsmAsync(moduleId, asm), Times.Once);
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(400);
        badRequestResult.Value.Should().BeOfType<ResponseModel>();
    }
    
    [Fact]
    public async Task GetAsmDetail_ShoulReturnCorrectData()
    {
        // Arrange
        var id = 1;
        var expectedResult = _fixture.Create<AssignmentViewModel>();
        _assignmentServiceMock.Setup(x => x.GetAsmById(id)).ReturnsAsync(expectedResult);
        
        // Act
        var result = await _assignmentController.GetAsmDetail(id);
        
        // Assert
        _assignmentServiceMock.Verify(x => x.GetAsmById(id), Times.Once);
        var badRequestResult = result.Should().BeOfType<OkObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(200);
        badRequestResult.Value.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact]
    public async Task GetAsmDetail_ShoulReturnBadRequest()
    {
        // Arrange
        var id = 1;
        var expectedResult = _fixture.Create<AssignmentViewModel>();
        _assignmentServiceMock.Setup(x => x.GetAsmById(id)).ThrowsAsync(new Exception());
        
        // Act
        var result = await _assignmentController.GetAsmDetail(id);
        
        // Assert
        _assignmentServiceMock.Verify(x => x.GetAsmById(id), Times.Once);
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(400);
        badRequestResult.Value.Should().BeOfType<ResponseModel>();
    }
    
    [Fact]
    public async Task UpdateAsmAsync_ShoulReturnNotFound()
    {
        // Arrange
        var id = 1;
        AssignmentViewModel? expectedResult = null;
        _assignmentServiceMock.Setup(x => x.GetAsmById(id)).ReturnsAsync(expectedResult);
        
        // Act
        var result = await _assignmentController.GetAsmDetail(id);
        
        // Assert
        _assignmentServiceMock.Verify(x => x.GetAsmById(id), Times.Once);
        var notFoundResult = result.Should().BeOfType<NotFoundResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);
    }
    
    [Fact]
    public async Task SoftDeleteById_ShoulReturnOkResult()
    {
        // Arrange
        var id = 1;
        var expectedResult = _fixture.Create<ResponseModel>();
        _assignmentServiceMock.Setup(x => x.SoftDeleteAsmById(id)).ReturnsAsync(expectedResult);
        
        // Act
        var result = await _assignmentController.SoftDeleteById(id);
        
        // Assert
        _assignmentServiceMock.Verify(x => x.SoftDeleteAsmById(id), Times.Once);
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact]
    public async Task SoftDeleteById_ShoulReturnBadRequest()
    {
        // Arrange
        var id = 1;
        _assignmentServiceMock.Setup(x => x.SoftDeleteAsmById(id)).ThrowsAsync(new Exception());
        
        // Act
        var result = await _assignmentController.SoftDeleteById(id);
        
        // Assert
        _assignmentServiceMock.Verify(x => x.SoftDeleteAsmById(id), Times.Once);
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(400);
    }
}