using Application.ViewModels.ResponseModels;
using AutoFixture;
using AutoMapper;
using Castle.Components.DictionaryAdapter.Xml;
using Domain.Tests;
using FAMS_GROUP2.API.Controllers;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.ModuleModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.ViewModels.StudentModels;
using FAMS_GROUP2.Services.Services;
using Microsoft.AspNetCore.Routing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Identity.Client;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Controllers
{
    public class ModuleControllerTest : SetupTest
    {
        private readonly ModulesController _moduleController;
        private readonly Fixture _fixture;
        public ModuleControllerTest()
        {
            _moduleController = new ModulesController(_moduleServiceMock.Object);
            _fixture = new Fixture();
        }
        [Fact]
        public async Task CreateModule_ShouldReturnOk_WhenModuleAddedSuccessfully()
        {
            // Arrange
            var moduleViewModel = _fixture.Create<CreateModuleViewModel>();
            _moduleServiceMock.Setup(x => x.CreateModuleAsync(moduleViewModel))
                              .ReturnsAsync(new ResponseModel { Status = true, Message = "Add Module Successfully!!" });

            // Act
            var result = await _moduleController.CreateModuleAsync(moduleViewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var response = Assert.IsType<ResponseModel>(okResult.Value);
            Assert.True(response.Status);
            Assert.Equal("Add Module Successfully!!", response.Message);
        }
        [Fact]
        public async Task CreateModule_ShouldReturnBadRequest_WhenErrorAddingModule()
        {
            // Arrange
            var moduleViewModel = _fixture.Create<CreateModuleViewModel>();
            _moduleServiceMock.Setup(x => x.CreateModuleAsync(moduleViewModel))
                              .ReturnsAsync(new ResponseModel { Status = false, Message = "Error adding Module" });

            // Act
            var result = await _moduleController.CreateModuleAsync(moduleViewModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            var response = Assert.IsType<ResponseModel>(badRequestResult.Value);
            Assert.False(response.Status);
            Assert.Equal("Error adding Module", response.Message);
        }
        [Fact]
        public async Task GetAllModuleAsync_ShouldReturnListOfModuleDetailsModel()
        {
            // Arrange
            var mockModuleDetails = _fixture.CreateMany<ModuleDetailsModel>(3).ToList();
            _moduleServiceMock.Setup(x => x.GetAllModuleAsync()).ReturnsAsync(mockModuleDetails);

            // Act
            var result = await _moduleController.GetAllModuleAsync();

            // Assert          
            var okResult = Assert.IsType<OkObjectResult>(result);
            var moduleDetails = (List<ModuleDetailsModel>)okResult.Value;
            Assert.Equal(mockModuleDetails.Count, moduleDetails.Count);
        }
        [Fact]
        public async Task GetAllModuleAsync_ShouldReturnNotFound_WhenNoModulesFound()
        {
            // Arrange
            _moduleServiceMock.Setup(x => x.GetAllModuleAsync()).ReturnsAsync((List<ModuleDetailsModel>)null);
            // Act
            var result = await _moduleController.GetAllModuleAsync();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
        [Fact]
        public async Task GetModuleById_ShouldReturnModuleDetails_WhenModuleExists()
        {
            // Arrange
            var moduleId = 1;
            var expectedModule = _fixture.Create<ModuleDetailsModel>();
            _moduleServiceMock.Setup(x => x.GetModuleByIDAsync(moduleId)).ReturnsAsync(expectedModule);

            // Act
            var result = await _moduleController.GetModuleByID(moduleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var moduleDetails = Assert.IsType<ModuleDetailsModel>(okResult.Value);
            Assert.Equal(expectedModule, moduleDetails);
        }
        [Fact]
        public async Task GetModuleById_ShouldReturnNotFound_WhenModuleNotExists()
        {
            // Arrange
            var moduleId = 1;
            _moduleServiceMock.Setup(x => x.GetModuleByIDAsync(moduleId)).ReturnsAsync((ModuleDetailsModel)null);

            // Act
            var result = await _moduleController.GetModuleByID(moduleId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);

        }
        [Fact]
        public async Task UpdateModule_ShouldReturnOk_WhenModuleUpdatedSuccessfully()
        {
            // Arrange
            var moduleId = 1;
            var updatedModuleModel = _fixture.Create<UpdateModuleViewModel>();
            _moduleServiceMock.Setup(x => x.UpdateModuleAsync(moduleId, updatedModuleModel))
                              .ReturnsAsync(new ResponseModel { Status = true, Message = "Module updated successfully" }); // Giả lập module được cập nhật thành công

            // Act
            var result = await _moduleController.UpdateModule(moduleId, updatedModuleModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var response = Assert.IsType<ResponseModel>(okResult.Value);
            Assert.True(response.Status);
            Assert.Equal("Module updated successfully", response.Message);
        }
        [Fact]
        public async Task UpdateModule_ShouldReturnNotFound_WhenModuleNotFound()
        {
            // Arrange
            var moduleId = 1;
            var updatedModuleModel = _fixture.Create<UpdateModuleViewModel>();
            _moduleServiceMock.Setup(x => x.UpdateModuleAsync(moduleId, updatedModuleModel))
                              .ReturnsAsync(new ResponseModel { Status = false, Message = "Module does not exist" }); // Giả lập không tìm thấy module để cập nhật

            // Act
            var result = await _moduleController.UpdateModule(moduleId, updatedModuleModel);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            var response = Assert.IsType<ResponseModel>(notFoundResult.Value);
            Assert.False(response.Status);
            Assert.Equal("Module does not exist", response.Message);
        }
        [Fact]
        public async Task DeleteModule_ShouldReturnSuccess()
        {
            // Arrange
            var moduleId = 1;
            _moduleServiceMock.Setup(x => x.DeleteModuleAsync(moduleId))
                              .ReturnsAsync(new ResponseModel { Status = true, Message = "Module deleted successfully" });

            // Act
            var result = await _moduleController.DeleteModule(moduleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public async Task DeleteModule_ShouldReturnNotFound_WhenModuleNotFound()
        {
            // Arrange
            var moduleId = 1;
            _moduleServiceMock.Setup(x => x.DeleteModuleAsync(moduleId))
                              .ReturnsAsync(new ResponseModel { Status = false, Message = "Module does not exist" });

            // Act
            var result = await _moduleController.DeleteModule(moduleId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
        [Fact]
        public async Task DeleteModule_ShouldReturnNotFound_WhenModuleIsUsed()
        {
            // Arrange
            var moduleId = 1;
            _moduleServiceMock.Setup(x => x.DeleteModuleAsync(moduleId))
                              .ReturnsAsync(new ResponseModel { Status = false, Message = "There is a class that is using module" });

            // Act
            var result = await _moduleController.DeleteModule(moduleId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            var response = Assert.IsType<ResponseModel>(notFoundResult.Value);
            Assert.Equal("There is a class that is using module", response.Message);
        }
        [Fact]
        public async Task PauseModule_ShouldReturnSuccess()
        {
            // Arrange
            var moduleId = 1;
            _moduleServiceMock.Setup(x => x.PauseModuleAsync(moduleId))
                              .ReturnsAsync(new ResponseModel { Status = true, Message = "Module is paused" });

            // Act
            var result = await _moduleController.PauseModule(moduleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public async Task PauseModule_ShouldReturnBadRequest_WhenModuleIsUsed()
        {
            // Arrange
            var moduleId = 1;
            _moduleServiceMock.Setup(x => x.PauseModuleAsync(moduleId))
                              .ReturnsAsync(new ResponseModel { Status = false, Message = "There is a class that is using module" });

            // Act
            var result = await _moduleController.PauseModule(moduleId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        [Fact]
        public async Task PauseModule_ShouldReturnNotFound_WhenModuleDoesNotExist()
        {
            // Arrange
            var moduleId = 1;
            _moduleServiceMock.Setup(x => x.PauseModuleAsync(moduleId))
                              .ReturnsAsync(new ResponseModel { Status = false, Message = "Module does not exist" });

            // Act
            var result = await _moduleController.PauseModule(moduleId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        [Fact]
        public async Task GetModuleByFilters_ShouldReturnCorrectData()
        {
            // Arrange
            var paginationParameter = _fixture.Create<PaginationParameter>();
            var moduleFilterModel = _fixture.Create<ModuleFilterModule>();
            var modules = _fixture.CreateMany<ModuleDetailsModel>(10).ToList();
            var expectedResult = new Pagination<ModuleDetailsModel>(modules, 10, 1, 1);
            _moduleServiceMock.Setup(x => x.GetPaginationAsync(paginationParameter, moduleFilterModel))
                               .ReturnsAsync(expectedResult);
            var httpContext = new DefaultHttpContext();
            var response = new Mock<HttpResponse>();
            var headers = new HeaderDictionary(); // Initialize headers
            headers.Add("X-Pagination", ""); // Add X-Pagination header
            response.SetupGet(r => r.Headers).Returns(headers); // Set the value for "X-Pagination"
            var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            var controllerContext = new ControllerContext(actionContext);
            // Act
            _moduleController.ControllerContext = controllerContext;
            var result = await _moduleController.GetModuleByFilters(paginationParameter, moduleFilterModel);
            // Assert
            _moduleServiceMock.Verify(x => x.GetPaginationAsync(paginationParameter, moduleFilterModel), Times.Once);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedResult);
        }










    }
}
