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
    public class LessonRepositoryTests : SetupTest
    {
        private readonly ILessonRepository _lessonRepository;
        public LessonRepositoryTests()
        {
            _lessonRepository = new LessonRepository(_dbContext, _currentTimeMock.Object, _claimsServiceMock.Object);
        }
        [Fact]
        public async Task AddCreateAsync_Should_ReturnCorrectData()
        {
            // Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Create mock Lesson
            var mockLesson = _fixture.Build<Lesson>()
                .Without(l => l.Module)
                .Without(l => l.Documents)
                .CreateMany(10).ToList();
            await _dbContext.Lessons.AddRangeAsync(mockLesson);
            // Act
            var saveChanges = await _dbContext.SaveChangesAsync();
            var Result = await _lessonRepository.GetAllAsync();
            // Assert
            saveChanges.Should().Be(mockLesson.Count());
            Result.Should().BeEquivalentTo(mockLesson);
        }
        [Fact]
        public async Task GetLessonDetails_ExistingModule_ShouldReturnCorrectData()
        {
            // Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Create mock Lesson
            var mockLesson = _fixture.Build<Lesson>()
                .Without(l => l.Module)
                .Without(l => l.Documents)
                .Create();
            // Act
            var addedLesson = await _lessonRepository.AddAsync(mockLesson);
            await _dbContext.SaveChangesAsync();
            var result = await _lessonRepository.GetByIdAsync(addedLesson.Id);
            // Assert
            result.Should().BeEquivalentTo(addedLesson);
        }
        [Fact]
        public async Task UpdateLesson_Should_ReturnCorrectData()
        {
            // Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            //Create mock Lesson
            var mockLesson = _fixture.Build<Lesson>()
                .Without(l => l.Module)
                .Without(l => l.Documents)
                .Create();
            await _lessonRepository.AddAsync(mockLesson);
            await _dbContext.SaveChangesAsync();
            // Act
            mockLesson.LessonName = "New Lesson";
            await _lessonRepository.Update(mockLesson);
            await _dbContext.SaveChangesAsync();
            var result = await _lessonRepository.GetByIdAsync(mockLesson.Id);
            // Assert
            result.Should().BeEquivalentTo(mockLesson);
            result.LessonName.Should().Be(mockLesson.LessonName);
        }
        [Fact]
        public async Task DeleteLessonAsync_Should_ReturnCorrectData()
        {
            // Arrange
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            //Create mock Lesson
            var mockLesson = _fixture.Build<Lesson>()
                .With(m => m.IsDelete, false)
                .Without(l => l.Module)
                .Without(l => l.Documents)
                .Create();
            await _dbContext.Lessons.AddAsync(mockLesson);
            await _dbContext.SaveChangesAsync();
            // Act
            await _lessonRepository.SoftRemove(mockLesson);
            var saveChanges = await _dbContext.SaveChangesAsync();
            var result = await _lessonRepository.GetByIdAsync(mockLesson.Id);
            // Assert
            saveChanges.Should().Be(1);
            result.Should().NotBeNull();
            result?.IsDelete.Should().BeTrue();
        }
    }
}
