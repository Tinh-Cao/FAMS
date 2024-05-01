using Application.ViewModels.ResponseModels;
using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.ViewModels.ModuleModels;
using FAMS_GROUP2.Repositories.ViewModels.ProgramModels;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Services.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;


namespace WebAPI.Tests.Services
{
    public class ModuleServiceTests : SetupTest
    {
        private readonly IModuleService _moduleService;

        public ModuleServiceTests()
        {
            _moduleService = new ModuleService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task CreateModuleAsync_Should_ReturnCorrectData()
        {
            // Arrange
            var moduleViewModel = _fixture.Create<CreateModuleViewModel>();

            var mockModule = _mapperConfig.Map<Module>(moduleViewModel);

            _unitOfWorkMock.Setup(uow => uow.ModuleRepository.AddAsync(It.IsAny<Module>()))
                .ReturnsAsync(mockModule);

            // Act
            var result = await _moduleService.CreateModuleAsync(moduleViewModel);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeTrue();
            result.Message.Should().Be("Add Module Successfully!!");
            _unitOfWorkMock.Verify(uow => uow.ModuleRepository.AddAsync(It.IsAny<Module>()), Times.Once());
            _unitOfWorkMock.Verify(uow => uow.SaveChangeAsync(), Times.Once());
        }
        [Fact]
        public async Task CreateModuleAsync_Should_ReturnError_WhenAddModuleFails()
        {
            // Arrange
            var moduleViewModel = _fixture.Create<CreateModuleViewModel>();

            _unitOfWorkMock.Setup(uow => uow.ModuleRepository.AddAsync(It.IsAny<Module>()))
                           .ReturnsAsync((Module)null);

            // Act
            var result = await _moduleService.CreateModuleAsync(moduleViewModel);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeFalse();
            result.Message.Should().Be("Error adding Module");
            _unitOfWorkMock.Verify(uow => uow.ModuleRepository.AddAsync(It.IsAny<Module>()), Times.Once());
        }
        [Fact]
        public async Task GetModuleByIDAsync_Should_ReturnModuleDetails_WhenModuleExists()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Arrange
            var moduleId = 1;
            var mockModule = _fixture.Create<Module>();
            _unitOfWorkMock.Setup(u => u.ModuleRepository.GetByIdAsync(moduleId)).ReturnsAsync(mockModule);
            var expectedModuleDetails = _mapperConfig.Map<ModuleDetailsModel>(mockModule);

            // Act
            var result = await _moduleService.GetModuleByIDAsync(moduleId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedModuleDetails);
            _unitOfWorkMock.Verify(u => u.ModuleRepository.GetByIdAsync(moduleId), Times.Once);
        }
        [Fact]
        public async Task GetModuleByIDAsync_Should_ReturnNull_WhenModuleDoesNotExist()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
           .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Arrange
            // Arrange
            var moduleId = 1;
            _unitOfWorkMock.Setup(u => u.ModuleRepository.GetByIdAsync(moduleId)).ReturnsAsync((Module)null);

            // Act
            var result = await _moduleService.GetModuleByIDAsync(moduleId);

            // Assert
            result.Should().BeNull();

            _unitOfWorkMock.Verify(u => u.ModuleRepository.GetByIdAsync(moduleId), Times.Once);
        }
        [Fact]
        public async Task GetAllModuleAsync_Should_ReturnModules_WhenModulesExist()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Arrange
            var mockModules = _fixture.Build<Module>().With(m => m.Status, "Active").With(m => m.IsDelete, false).CreateMany(2).ToList();
            _unitOfWorkMock.Setup(u => u.ModuleRepository.GetAllAsync()).ReturnsAsync(mockModules);

            var expectedModules = _mapperConfig.Map<List<ModuleDetailsModel>>(mockModules);

            // Act
            var result = await _moduleService.GetAllModuleAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedModules);

            _unitOfWorkMock.Verify(u => u.ModuleRepository.GetAllAsync(), Times.Once);
        }
        [Fact]
        public async Task GetAllModuleAsync_Should_ReturnEmptyList_WhenNoModulesExist()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Arrange
            _unitOfWorkMock.Setup(u => u.ModuleRepository.GetAllAsync()).ReturnsAsync((List<Module>)null);

            // Act
            var result = await _moduleService.GetAllModuleAsync();

            // Assert
            result.Should().BeNull();

            _unitOfWorkMock.Verify(u => u.ModuleRepository.GetAllAsync(), Times.Once);
        }
        [Fact]
        public async Task DeleteModuleAsync_Should_ReturnSuccess_WhenModuleIsNotUsedAndDeleted()
        {
            // Arrange
            var moduleId = 1;
            _unitOfWorkMock.Setup(u => u.ModuleRepository.isModuleUsed(moduleId)).ReturnsAsync(false);
            _unitOfWorkMock.Setup(u => u.ModuleRepository.GetByIdAsync(moduleId)).ReturnsAsync(new Module());

            // Act
            var result = await _moduleService.DeleteModuleAsync(moduleId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeTrue();
            result.Message.Should().Be("Module deleted successfully");
            _unitOfWorkMock.Verify(u => u.ModuleRepository.SoftRemove(It.IsAny<Module>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangeAsync(), Times.Once);
        }
        [Fact]
        public async Task DeleteModuleAsync_Should_ReturnError_WhenModuleIsUsed()
        {
            // Arrange
            var moduleId = 1;
            _unitOfWorkMock.Setup(u => u.ModuleRepository.isModuleUsed(moduleId)).ReturnsAsync(true);

            // Act
            var result = await _moduleService.DeleteModuleAsync(moduleId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeFalse();
            result.Message.Should().Be("There is a class that is using module");
            _unitOfWorkMock.Verify(u => u.ModuleRepository.isModuleUsed(moduleId), Times.Once);
            _unitOfWorkMock.Verify(u => u.ModuleRepository.SoftRemove(It.IsAny<Module>()), Times.Never);

        }

        [Fact]
        public async Task UpdateModuleAsync_Should_ReturnSuccess_WhenModuleExistsAndNotDeleted()
        {
            // Arrange
            var moduleId = 1;
            var moduleModel = new UpdateModuleViewModel
            {
                ModuleCode = "MOD001",
                ModuleName = "Updated Module",
                Status = "Active"
            };
            var module = new Module
            {
                Id = moduleId,
                ModuleCode = "Old Code",
                ModuleName = "Old Module",
                Status = "Inactive",
                IsDelete = false
            };
            _unitOfWorkMock.Setup(u => u.ModuleRepository.GetByIdAsync(moduleId)).ReturnsAsync(module);

            // Act
            var result = await _moduleService.UpdateModuleAsync(moduleId, moduleModel);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeTrue();
            module.ModuleCode.Should().Be(moduleModel.ModuleCode);
            module.ModuleName.Should().Be(moduleModel.ModuleName);
            module.Status.Should().Be(moduleModel.Status);
        }
        [Fact]
        public async Task UpdateModuleAsync_Should_ReturnError_WhenModuleNotFound()
        {
            // Arrange
            var moduleId = 1;
            var moduleModel = new UpdateModuleViewModel
            {
                ModuleCode = "MOD001",
                ModuleName = "Updated Module",
                Status = "Active"
            };
            _unitOfWorkMock.Setup(u => u.ModuleRepository.GetByIdAsync(moduleId)).ReturnsAsync((Module)null);

            // Act
            var result = await _moduleService.UpdateModuleAsync(moduleId, moduleModel);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeFalse();
            result.Message.Should().Be("Module does not exist");
            _unitOfWorkMock.Verify(u => u.ModuleRepository.Update(It.IsAny<Module>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangeAsync(), Times.Never);
        }


    }
}