using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.ViewModels.LessonModels;
using FAMS_GROUP2.Repositories.ViewModels.ModuleModels;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Services.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Services
{
    public class LessonServiceTests : SetupTest
    {
        private readonly ILessonService _lessonService;
        private readonly Fixture _fixture;
        public LessonServiceTests()
        {
            _lessonService = new LessonService(_mapperConfig, _unitOfWorkMock.Object);
            _fixture = new Fixture();
        }
        [Fact]
        public async Task CreateLessonAsync_Should_ReturnCorrectData()
        {
            // Arrange
            var lessonModel = _fixture.Create<CreateLessonModel>();

            var mockLesson = _mapperConfig.Map<Lesson>(lessonModel);
            _unitOfWorkMock.Setup(a => a.LessonRepository.AddAsync(It.IsAny<Lesson>()))
                           .ReturnsAsync(mockLesson);
            // Act
            var result = await _lessonService.CreateLessonAsync(lessonModel);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeTrue();
            result.Message.Should().Be("Added Lesson Successfully");
            _unitOfWorkMock.Verify(a => a.LessonRepository.AddAsync(It.IsAny<Lesson>()), Times.Once());
            _unitOfWorkMock.Verify(a => a.SaveChangeAsync(), Times.Once());
        }
        [Fact]
        public async Task CreateLessonAsync_Should_ReturnError_WhenAddLessonFails()
        {
            // Arrange
            var lessonModel = _fixture.Create<CreateLessonModel>();

            _unitOfWorkMock.Setup(a => a.LessonRepository.AddAsync(It.IsAny<Lesson>()))
                           .ReturnsAsync((Lesson)null);
            // Act
            var result = await _lessonService.CreateLessonAsync(lessonModel);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeFalse();
            result.Message.Should().Be("Error adding!!");
            _unitOfWorkMock.Verify(a => a.LessonRepository.AddAsync(It.IsAny<Lesson>()), Times.Once());
        }
        [Fact]
        public async Task GetLessonByIdAsync_Should_ReturnLessonDetails_WhenLessonExists()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Arrange
            var lessonId = 1;
            var mockLesson = _fixture.Create<Lesson>();
            _unitOfWorkMock.Setup(a => a.LessonRepository.GetByIdAsync(lessonId)).ReturnsAsync(mockLesson);
            var expectedLessonDetails = _mapperConfig.Map<LessonDetailsModel>(mockLesson);

            // Act
            var result = await _lessonService.GetLessonByIdAsync(lessonId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedLessonDetails);
            _unitOfWorkMock.Verify(a => a.LessonRepository.GetByIdAsync(lessonId), Times.Once);
        }
        [Fact]
        public async Task GetLessonyIdAsync_Should_ReturnNull_WhenLessonDoesNotExist()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
           .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Arrange
            // Arrange
            var lessonId = 1;
            _unitOfWorkMock.Setup(a => a.LessonRepository.GetByIdAsync(lessonId)).ReturnsAsync((Lesson)null);

            // Act
            var result = await _lessonService.GetLessonByIdAsync(lessonId);

            // Assert
            result.Should().BeNull();

            _unitOfWorkMock.Verify(a => a.LessonRepository.GetByIdAsync(lessonId), Times.Once);
        }
        [Fact]
        public async Task GetAllLessonAsync_Should_ReturnLessons_WhenLessonExist()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Arrange
            var mockLesson = _fixture.Build<Lesson>().With(l => l.Status, "OnGoing").With(m => m.IsDelete, false).CreateMany(2).ToList();
            _unitOfWorkMock.Setup(a => a.LessonRepository.GetAllAsync()).ReturnsAsync(mockLesson);

            var expectedModules = _mapperConfig.Map<List<LessonDetailsModel>>(mockLesson);

            // Act
            var result = await _lessonService.GetAllLessonAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedModules);

            _unitOfWorkMock.Verify(a => a.LessonRepository.GetAllAsync(), Times.Once);
        }
        [Fact]
        public async Task GetAllLessonAsync_Should_ReturnEmptyList_WhenNoLessonsExist()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Arrange
            _unitOfWorkMock.Setup(a => a.LessonRepository.GetAllAsync()).ReturnsAsync((List<Lesson>)null);

            // Act
            var result = await _lessonService.GetAllLessonAsync();

            // Assert
             result.Should().BeEmpty();
            _unitOfWorkMock.Verify(a => a.LessonRepository.GetAllAsync(), Times.Once);
        }
        [Fact]
        public async Task DeleteLessonAsync_Should_ReturnSuccess_WhenLessonisDelete()
        {
            // Arrange
            var lessonId = 1;
            _unitOfWorkMock.Setup(a => a.LessonRepository.GetByIdAsync(lessonId)).ReturnsAsync(new Lesson());

            // Act
            var result = await _lessonService.DeleteLessonAsync(lessonId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeTrue();
            result.Message.Should().Be("Lesson deleted successfully");
            _unitOfWorkMock.Verify(a => a.LessonRepository.SoftRemove(It.IsAny<Lesson>()), Times.Once);
            _unitOfWorkMock.Verify(a => a.SaveChangeAsync(), Times.Once);
        }
        [Fact]
        public async Task UpdateLessonAsync_Should_ReturnSuccess_WhenLessonExistsAndNotDeleted()
        {
            // Arrange
            var lessonId = 1;
            var lessonModel = new UpdateLessonModel
            {
                LessonCode = "LE001",
                LessonName = "Updated Lesson",
                Status = "OnGoing"
            };
            var lesson = new Lesson
            {
                Id = lessonId,
                LessonCode = "Old Code",
                LessonName = "Old Lesson",
                Status = "Finished",
                IsDelete = false
            };
            _unitOfWorkMock.Setup(a => a.LessonRepository.GetByIdAsync(lessonId)).ReturnsAsync(lesson);

            // Act
            var result = await _lessonService.UpdateLessonAsync(lessonId, lessonModel);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeFalse();
            lesson.LessonCode.Should().Be(lessonModel.LessonCode);
            lesson.LessonName.Should().Be(lessonModel.LessonName);
            lesson.Status.Should().Be(lessonModel.Status);
        }
    }
}
