using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Repositories;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Repositories
{
    public class ModuleRepositoryTests : SetupTest
    {
        private readonly IModuleRepository _moduleRepository;
        public ModuleRepositoryTests()
        {
            _moduleRepository = new ModuleRepository(_dbContext, _currentTimeMock.Object, _claimsServiceMock.Object);
        }
        [Fact]
        public async Task AddModuleAsync_Should_ReturnCorrectData()
        {
            // ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Tạo dữ liệu mock
            var mockModule = _fixture.Build<Module>()
                .Without(m => m.Assignments)
                .Without(m => m.Lessons)
                .Without(m => m.ProgramModules)
                .Create();

            // ACT
            await _moduleRepository.AddAsync(mockModule);
            var saveChanges = await _dbContext.SaveChangesAsync();
            var result = await _moduleRepository.GetAllAsync();

            // ASSERT
            saveChanges.Should().Be(1);
            result.Should().ContainEquivalentOf(mockModule);
        }
        [Fact]
        public async Task GetModuleDetails_ExistingModule_ShouldReturnCorrectData()
        {
            // ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Tạo dữ liệu mock 
            var mockModule = _fixture.Build<Module>()
                .Without(m => m.Assignments)
                .Without(m => m.Lessons)
                .Without(m => m.ProgramModules)
                .Create();

            await _moduleRepository.AddAsync(mockModule);
            await _dbContext.SaveChangesAsync();

            // ACT
            var result = await _moduleRepository.GetByIdAsync(mockModule.Id);

            // ASSERT
            result.Should().BeEquivalentTo(mockModule);
        }
        [Fact]
        public async Task UpdateModule_Should_ReturnCorrectData()
        {
            // ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Tạo dữ liệu mock 
            var mockModule = _fixture.Build<Module>()
                .Without(m => m.Assignments)
                .Without(m => m.Lessons)
                .Without(m => m.ProgramModules)
                .Create();
            await _moduleRepository.AddAsync(mockModule);
            await _dbContext.SaveChangesAsync();

            // ACT
            mockModule.ModuleName = "New Module";
            await _moduleRepository.Update(mockModule);
            await _dbContext.SaveChangesAsync();
            var result = await _moduleRepository.GetByIdAsync(mockModule.Id);

            // ASSERT
            result.Should().BeEquivalentTo(mockModule);
            result.ModuleName.Should().Be(mockModule.ModuleName);
        }
        [Fact]
        public async Task DeleteModuleAsync_Should_ReturnCorrectData()
        {
            // ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var mockModule = _fixture.Build<Module>()
            .With(m => m.IsDelete, false)
            .Without(m => m.Assignments)
            .Without(m => m.Lessons)
            .Without(m => m.ProgramModules)
            .Create();
            await _dbContext.Modules.AddAsync(mockModule);
            await _dbContext.SaveChangesAsync();

            // Act   
            await _moduleRepository.SoftRemove(mockModule);
            var saveChanges = await _dbContext.SaveChangesAsync();
            var result = await _moduleRepository.GetByIdAsync(mockModule.Id);

            // Assert
            saveChanges.Should().Be(1);
            result.Should().NotBeNull();
            result?.IsDelete.Should().BeTrue();
        }
    }

}

