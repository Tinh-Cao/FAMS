using Application.ViewModels.ResponseModels;
using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.API.Controllers;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.ProgramModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Controllers
{
    public class ProgramControllerTests : SetupTest
    {
        private readonly Fixture _fixure;
        private readonly ProgramController _programController;
        public ProgramControllerTests()
        {
            _fixure = new Fixture();
            _programController = new ProgramController(_programServiceMock.Object);
        }
        // unit test for createProgramAsync
        [Fact]
        public async Task CreateProgramAsync_WhenProgramServiceReturnsNull_ShouldReturnBadRequest()
        {
            // Arrange
            var programModelMock = _fixture.Create<ProgramModel>();
            //var programModelResponse = _fixture.Build<TrainingProgram>().Create();
            _programServiceMock.Setup(x => x.CreateProgramAsync(programModelMock)).ReturnsAsync((TrainingProgram)null);


            // Act
            var result = await _programController.CreateProgramAsync(programModelMock);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var responseModel = Assert.IsType<ResponseModel>(badRequestResult.Value);
            Assert.False(responseModel.Status);
            Assert.Equal("Training Program Code is already existed!!!", responseModel.Message);
        }

        [Fact]
        public async Task CreateProgramAsync_WhenProgramServiceReturnsNotNull_ShouldReturnOk()
        {
            //Arrange
            var programModelMock = _fixture.Create<ProgramModel>();
            _programServiceMock.Setup(x => x.CreateProgramAsync(programModelMock)).ReturnsAsync(new TrainingProgram());


            //Act
            var result = await _programController.CreateProgramAsync(programModelMock);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseModel = Assert.IsType<ResponseModel>(okResult.Value);
            Assert.True(responseModel.Status);
            Assert.Equal("Add training program succesfully", responseModel.Message);
        }

        [Fact]
        public async Task CreateProgramAsync_WhenExceptionThrown_ShouldReturnBadRequestWithErrorMessage()
        {
            // Arrange
            var programModelMock = _fixture.Create<ProgramModel>();
            _programServiceMock.Setup(x => x.CreateProgramAsync(programModelMock)).ThrowsAsync(new Exception("Test exception"));


            // Act
            var result = await _programController.CreateProgramAsync(programModelMock);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test exception", badRequestResult.Value);
        }
        // unit test for GetProgramById
        [Fact]
        public async Task GetProgramById_WhenProgramExists_ShouldReturnOk()
        {
            // Arrange
            int programId = 1;
            var programResponse = _fixture.Create<ProgramResponseModel>();
            _programServiceMock.Setup(x => x.GetProgramByIdAsync(programId)).ReturnsAsync(programResponse);

            // Act
            var result = await _programController.GetProgramById(programId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseModel = Assert.IsType<ProgramResponseModel>(okResult.Value);
            Assert.Equal(programResponse, responseModel);
        }

        [Fact]
        public async Task GetProgramById_WhenProgramDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            int programId = 2;
            _programServiceMock.Setup(x => x.GetProgramByIdAsync(programId)).ReturnsAsync((ProgramResponseModel)null);

            // Act
            var result = await _programController.GetProgramById(programId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetProgramById_WhenExceptionThrown_ShouldReturnBadRequestWithErrorMessage()
        {
            // Arrange
            int programId = 3;
            _programServiceMock.Setup(x => x.GetProgramByIdAsync(programId)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _programController.GetProgramById(programId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test exception", badRequestResult.Value);
        }
        // unit test for updateprogram
        [Fact]
        public async Task UpdateProgram_WhenProgramAndModulesUpdated_ShouldReturnOkWithMessage()
        {
            // Arrange
            int programId = 1;
            var updateProgramModel = _fixture.Create<UpdateProgramModel>();
            _programServiceMock.Setup(x => x.UpdateProgramAsync(programId, updateProgramModel)).ReturnsAsync(1);

            // Act
            var result = await _programController.UpdateProgram(programId, updateProgramModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseModel = Assert.IsType<ResponseModel>(okResult.Value);
            Assert.True(responseModel.Status);
            Assert.Equal("Update successfully", responseModel.Message);
        }

        //[Fact]
        //public async Task UpdateProgram_WhenProgramUpdatedWithoutModules_ShouldReturnOkWithMessage()
        //{
        //    // Arrange
        //    int programId = 2;
        //    var updateProgramModel = _fixture.Create<UpdateProgramModel>();
        //    _programServiceMock.Setup(x => x.UpdateProgramAsync(programId, updateProgramModel)).ReturnsAsync(2);

        //    // Act
        //    var result = await _programController.UpdateProgram(programId, updateProgramModel);

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var responseModel = Assert.IsType<ResponseModel>(okResult.Value);
        //    Assert.True(responseModel.Status);
        //    Assert.Equal("Update TP successfully!!!", responseModel.Message);
        //}

        [Fact]
        public async Task UpdateProgram_WhenProgramNotFound_ShouldReturnNotFound()
        {
            // Arrange
            int programId = 3;
            var updateProgramModel = _fixture.Create<UpdateProgramModel>();
            _programServiceMock.Setup(x => x.UpdateProgramAsync(programId, updateProgramModel)).ReturnsAsync(2);

            // Act
            var result = await _programController.UpdateProgram(programId, updateProgramModel);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateProgram_WhenExceptionThrown_ShouldReturnBadRequestWithErrorMessage()
        {
            // Arrange
            int programId = 4;
            var updateProgramModel = _fixture.Create<UpdateProgramModel>();
            _programServiceMock.Setup(x => x.UpdateProgramAsync(programId, updateProgramModel)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _programController.UpdateProgram(programId, updateProgramModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test exception", badRequestResult.Value);
        }

        // unit test for DeleteProgramAsync
        [Fact]
        public async Task DeleteProgramAsync_WhenProgramDeletedSuccessfully_ShouldReturnOkWithMessage()
        {
            // Arrange
            int programId = 2;
            _programServiceMock.Setup(x => x.DeleteProgramAsync(programId)).ReturnsAsync(1);

            // Act
            var result = await _programController.DeleteProgramAsync(programId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseModel = Assert.IsType<ResponseModel>(okResult.Value);
            Assert.True(responseModel.Status);
            Assert.Equal("Delele successfully!!!", responseModel.Message);
        }

        [Fact]
        public async Task DeleteProgramAsync_WhenProgramNotFound_ShouldReturnNotFound()
        {
            // Arrange
            int programId = 3;
            _programServiceMock.Setup(x => x.DeleteProgramAsync(programId)).ReturnsAsync(0);

            // Act
            var result = await _programController.DeleteProgramAsync(programId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteProgramAsync_WhenExceptionThrown_ShouldReturnBadRequestWithErrorMessage()
        {
            // Arrange
            int programId = 4;
            _programServiceMock.Setup(x => x.DeleteProgramAsync(programId)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _programController.DeleteProgramAsync(programId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test exception", badRequestResult.Value);
        }
    

        [Fact]
        public async Task GetProgramsAsync_WhenNoProgramsFound_ShouldReturnOkWithEmptyList()
        {
            // Arrange
            var paginationParameter = _fixture.Create<PaginationParameter>();
            var programFilterModel = _fixture.Create<ProgramFilterModel>();
            _programServiceMock
                .Setup(x => x.GetProgramsByFiltersAsync(paginationParameter, programFilterModel))
                .ReturnsAsync((Pagination<ProgramResponseModel>)null);

            // Act
            var result = await _programController.getProgramsAsync(paginationParameter, programFilterModel);

            // Assert

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async Task GetProgramsAsync_WhenExceptionThrown_ShouldReturnBadRequestWithErrorMessage()
        {
            // Arrange
            var paginationParameter = new PaginationParameter();
            var programFilterModel = new ProgramFilterModel();
            _programServiceMock
                .Setup(x => x.GetProgramsByFiltersAsync(paginationParameter, programFilterModel))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _programController.getProgramsAsync(paginationParameter, programFilterModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test exception", badRequestResult.Value);
        }
    }
}

