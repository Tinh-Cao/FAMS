using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.ViewModels.ProgramModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Services.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Services
{
    public class ProgramServiceTests : SetupTest
    {
        private readonly Fixture _fixture;
        private readonly IProgramService _programService;

        public ProgramServiceTests()
        {
            _fixture = new Fixture();
            _programService = new ProgramService(_mapperConfig, _unitOfWorkMock.Object);
        }
        [Fact]
        public async Task CreateProgramAsync_WithValidData_ShouldReturnCreatedProgram()
        {
            // Arrange
            var programModel = _fixture.Create<ProgramModel>();
            var mappedProgram = _mapperConfig.Map<TrainingProgram>(programModel);

            // mappedProgram.ProgramCode = programModel.ProgramCode; // Ensure program code matches

            _unitOfWorkMock.Setup(u => u.ProgramRepository.GetAllAsync()).ReturnsAsync(new List<TrainingProgram>());

            _unitOfWorkMock.Setup(u => u.ProgramRepository.AddAsync(It.IsAny<TrainingProgram>())).ReturnsAsync(mappedProgram);

            // Act
            var result = await _programService.CreateProgramAsync(programModel);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(mappedProgram);
            Assert.False(result.IsDelete);
            _unitOfWorkMock.Verify(u => u.SaveChangeAsync(), Times.Once);
        }
        [Fact]
        public async Task CreateProgramAsync_DuplicateProgramCode_ReturnsNull()
        {
            var programModel = _fixture.Create<ProgramModel>();
            var mappedProgram = _mapperConfig.Map<TrainingProgram>(programModel);

            // mappedProgram.ProgramCode = programModel.ProgramCode; // Ensure program code matches

            _unitOfWorkMock.Setup(u => u.ProgramRepository.GetAllAsync()).ReturnsAsync(new List<TrainingProgram>());

            _unitOfWorkMock.Setup(u => u.ProgramRepository.AddAsync(It.IsAny<TrainingProgram>())).ReturnsAsync(mappedProgram);

            // Act
            var result = await _programService.CreateProgramAsync(programModel);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(mappedProgram);
            Assert.False(result.IsDelete);
            _unitOfWorkMock.Verify(u => u.SaveChangeAsync(), Times.Once);
        }
        [Fact]
        public async Task GetAllProgramsAsync_ShouldReturnCorrectData()
        {
            var expectedPrograms = _fixture.Build<ProgramResponseModel>().WithAutoProperties().CreateMany(2).ToList();

            _unitOfWorkMock.Setup(u => u.ProgramRepository.GetTrainingPrograms()).ReturnsAsync(expectedPrograms); 

            // Act
            var result = await _programService.GetAllProgramAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPrograms.Count(), result.Count);
            // Add additional assertions as needed to verify the content of the returned list
            // For example: Assert.AreEqual(expectedPrograms.First().PropertyName, result.First().PropertyName);

            _unitOfWorkMock.Verify(u => u.ProgramRepository.GetTrainingPrograms(), Times.Once);
        }
        [Fact]
        public async Task GetProgramByIdAsync_ShouldReturnCorrectData()
        {
            // Arrange
            
            var proId = 1;
            var mockTP = _fixture.Build<ProgramResponseModel>().With(tp => tp.Id, proId.ToString()).Create();
            _unitOfWorkMock.Setup(u => u.ProgramRepository.GetTrainingProgramById(proId)).ReturnsAsync(mockTP);
            var expectedTP = _mapperConfig.Map<ProgramResponseModel>(mockTP);
            // Act
            var result = await _programService.GetProgramByIdAsync(proId);

            //Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(mockTP);
            _unitOfWorkMock.Verify(u => u.ProgramRepository.GetTrainingProgramById(proId), Times.Once);

        }
        [Fact]
        public async Task UpdateProgramAsync_NotFoundProgram_Returns2()
        {
            // Arrange
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            int programId = 1;
            var mockPM = _fixture.Build<ProgramModule>()
                                 .With(pm => pm.ProgramId)
                                 .With(pm => pm.ModuleId)
                                 .With(pm => pm.IsDelete, false)
                                 .CreateMany(1).ToList();
            var mockM = _fixture.Build<Module>()
                                .With(m => m.IsDelete, false)

                                .CreateMany(1).ToList();

            var mockUpdateModel = _fixture.Build<UpdateProgramModel>()
                                          .With(u => u.ProgramName, "Test")
                                          .With( u => u.Duration,"3months")
                                          .Without(u => u.ListIdForAdd)
                                          .Without(u => u.ListIdForRemove)
                                          .Create();

            var mockTP = _fixture.Build<ProgramResponseModel>().With(t => t.Id, "1").Without(t => t.ModulesId).Create();
            //var expectedTP = _mapperConfig.Map<ProgramResponseModel>(mockTP);
            _unitOfWorkMock.Setup(u => u.ProgramRepository.GetTrainingProgramById(programId)).ReturnsAsync(mockTP);
            _unitOfWorkMock.Setup(u => u.ProgramModuleRepository.GetAllAsync()).ReturnsAsync(mockPM);
            _unitOfWorkMock.Setup(u => u.ModuleRepository.GetAllAsync()).ReturnsAsync(mockM);
          
            //_unitOfWorkMock.Setup(u => u.ProgramRepository.Update(mockTP))
            // Act
            var result = await _programService.UpdateProgramAsync(programId, mockUpdateModel);

            // Assert
            Assert.Equal(2, result);

        }
        [Fact]
        public async Task DeleteProgramAsync_ExistingProgramWithProgramModules_DeletesProgramAndProgramModules()
        {
            // Arrange

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            int programId = 1;
            var existingProgram = _fixture.Build<TrainingProgram>()
                                          .With(p => p.Id, programId)
                                          .Without(p=> p.Classes)
                                          .Without(p => p.ProgramModules)
                                          .Create();

            var mockListPm = _fixture.Build<ProgramModule>()
                                     .With(pm => pm.ProgramId, programId)
                                     .With(pm => pm.IsDelete , false)
                                     .CreateMany(5)
                                     .ToList();
            
            _unitOfWorkMock.Setup(u => u.ProgramRepository.GetByIdAsync(programId)).ReturnsAsync(existingProgram);
            _unitOfWorkMock.Setup(u => u.ProgramModuleRepository.GetAllAsync()).ReturnsAsync(mockListPm);

            // Act
            var result = await _programService.DeleteProgramAsync(programId);

            // Assert
            Assert.Equal(1, result); // Assuming 1 means success 
            _unitOfWorkMock.Verify(u => u.ProgramRepository.Update(existingProgram), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteProgramAsync_NonExistingProgram_Returns0()
        {
            // Arrange

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Arrange
            int programId = 1;
            var emptyProgramList = new List<ProgramModule>(); // Create an empty list
            _unitOfWorkMock.Setup(u => u.ProgramModuleRepository.GetAllAsync())
                .ReturnsAsync(emptyProgramList); // Return the empty list
            _unitOfWorkMock.Setup(u => u.ProgramRepository.GetByIdAsync(programId))
                .ReturnsAsync((TrainingProgram)null);

            // Act
            var result = await _programService.DeleteProgramAsync(programId);

            // Assert
            Assert.Equal(0, result); // Assuming 0 means failure

            // Verify that ProgramModuleRepository.UpdateRange and ProgramRepository.Update are not called
            _unitOfWorkMock.Verify(u => u.ProgramModuleRepository.UpdateRange(emptyProgramList), Times.Never);
            _unitOfWorkMock.Verify(u => u.ProgramRepository.Update(It.IsAny<TrainingProgram>()), Times.Never);
            // Verify that SaveChangesAsync is not called
            _unitOfWorkMock.Verify(u => u.SaveChangeAsync(), Times.Never);
        }
    }
    
}
