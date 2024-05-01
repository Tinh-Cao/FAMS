using Application.ViewModels.ResponseModels;
using AutoFixture;
using AutoMapper;
using Domain.Tests;
using FluentAssertions;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Services.Services;
using Moq;
using FAMS_GROUP2.Repositories.ViewModels.TokenModels;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.ScoreModels;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.ViewModels.ProgramModels;

namespace WebAPI.Tests.Services
{
    public class ScoreServiceTests : SetupTest
    {
        private readonly IScoreService _scoreService;

        public ScoreServiceTests()
        {
            _scoreService = new ScoreService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task GetScoresByFiltersAsync_ValidInput_ReturnsExpectedResult()
        {
            // Thay đổi hành vi của AutoFixture để bỏ qua các thuộc tính hoặc trường gây ra đệ quy
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Arrange
            var paginationParameter = new PaginationParameter { PageIndex = 1, PageSize = 10 };
            var scoreFilterModel = new ScoreFilterModel
            {
                ClassId = 1, // Lọc các điểm của lớp học có ID là 1
                Sort = "Status", // Sắp xếp các điểm theo ID
                SortDirection = "desc", // Sắp xếp theo thứ tự giảm dần
                IsDelete = false, // Chỉ lấy các điểm chưa bị xóa
                Status = "PASS", // Chỉ lấy các điểm có trạng thái là "Active"
                LevelModule = 2 // Chỉ lấy các điểm của mô-đun cấp độ 
            };

            // Create a list of scores using AutoFixture
            var scores = _fixture.Create<List<Score>>();
            var paginatedScores = new Pagination<Score>(scores, scores.Count, paginationParameter.PageIndex, paginationParameter.PageSize);

            // Set up the mock repository
            _unitOfWorkMock.Setup(x => x.ScoreRepository.GetScoresByFiltersAsync(paginationParameter, scoreFilterModel)).ReturnsAsync(paginatedScores);

            // Act
            var result = await _scoreService.GetScoresByFiltersAsync(paginationParameter, scoreFilterModel);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(scores.Count);
            result.CurrentPage.Should().Be(paginationParameter.PageIndex);
            result.PageSize.Should().Be(paginationParameter.PageSize);
            result.ToList().Count.Should().Be(scores.Count);
        }

        [Fact]
        public async Task GetScoreByIdAsync_ExistingScore_ReturnCorrectData()
        {
            // Arrange
            var studentId = 1;
            var classId = 1;            

            // Change AutoFixture behavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Mock repository response
            var scores = _fixture.Create<List<Score>>();
            _unitOfWorkMock.Setup(x => x.ScoreRepository.GetAllAsync()).ReturnsAsync(scores);
            var expectedScore = _mapperConfig.Map<ScoreViewModel>(scores.FirstOrDefault(s => s.StudentId == studentId && s.ClassId == classId));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).Returns(Task.FromResult(1));

            // Act
            var result = await _scoreService.GetScoreByIdAsync(studentId, classId);

            // Assert
            result.Should().BeEquivalentTo(expectedScore);
        }

        [Fact]
        public async Task AddScoreByFormAsync_NewScore_ReturnsSuccessMessage()
        {
            // Arrange
            var scoreModel = new ScoreCreateModel { StudentId = 1, ClassId = 1 };
            var scoreObj = _mapperConfig.Map<Score>(scoreModel);

            _unitOfWorkMock.Setup(x => x.ScoreRepository.GetScoreIdAsync(scoreModel.StudentId, scoreModel.ClassId)).ReturnsAsync((Score)null);
            _unitOfWorkMock.Setup(x => x.ScoreRepository.AddAsync(It.IsAny<Score>())).ReturnsAsync(scoreObj);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            // Act
            var result = await _scoreService.AddScoreByFormAsync(scoreModel);

            // Assert
            Assert.True(result.Status);
            Assert.Equal("Add Score Successfully!!", result.Message);
            _unitOfWorkMock.Verify(x => x.ScoreRepository.AddAsync(It.IsAny<Score>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }

        [Fact]
        public async Task AddScoreByFormAsync_ExistingScore_ReturnsErrorMessage()
        {
            // Arrange
            var scoreModel = new ScoreCreateModel { StudentId = 1, ClassId = 1 };

            // Simulate an existing score
            _unitOfWorkMock.Setup(x => x.ScoreRepository.GetScoreIdAsync(scoreModel.StudentId, scoreModel.ClassId)).ReturnsAsync(new Score());

            // Act
            var result = await _scoreService.AddScoreByFormAsync(scoreModel);

            // Assert
            Assert.False(result.Status);
            Assert.Equal("Score is already existed", result.Message);
        }

        [Fact]
        public async Task UpdateScoreByStudentAsync_ScoreExists_ReturnsSuccessMessage()
        {
            // Arrange
            var studentId = 1;
            var classId = 1;
            var scoreModel = new ScoreUpdateModel();
            var existingScore = new Score();

            _unitOfWorkMock.Setup(x => x.ScoreRepository.GetScoreIdAsync(studentId, classId)).ReturnsAsync(existingScore);
            _unitOfWorkMock.Setup(x => x.ScoreRepository.Update(existingScore)).Returns(Task.FromResult(true));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).Returns(Task.FromResult(1));

            // Act
            var result = await _scoreService.UpdateScoreByStudentAsync(studentId, classId, scoreModel);

            // Assert
            Assert.True(result.Status);
            Assert.Equal("Score updated successfully", result.Message);
            _unitOfWorkMock.Verify(x => x.ScoreRepository.Update(existingScore), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }

        [Fact]
        public async Task UpdateScoreByStudentAsync_ScoreDoesNotExist_ReturnsErrorMessage()
        {
            // Arrange
            var studentId = 1;
            var classId = 1;
            var scoreModel = new ScoreUpdateModel();

            // Simulate a non-existing score
            _unitOfWorkMock.Setup(x => x.ScoreRepository.GetScoreIdAsync(studentId, classId)).ReturnsAsync((Score)null);

            // Act
            var result = await _scoreService.UpdateScoreByStudentAsync(studentId, classId, scoreModel);

            // Assert
            Assert.False(result.Status);
            Assert.Equal("Invalid ID", result.Message);
        }

        [Fact]
        public async Task UpdateScoreByClassAsync_ScoresExist_ReturnsSuccessMessage()
        {
            // Arrange
            var scoreModels = new List<ScoreCreateModel>
            {
                new ScoreCreateModel { StudentId = 1, ClassId = 1 },
                new ScoreCreateModel { StudentId = 2, ClassId = 2 }
            };
            
            var existingScores = new List<Score>
            {
                new Score { StudentId = 1, ClassId = 1 },
                new Score { StudentId = 2, ClassId = 2 }
            };

            _unitOfWorkMock.Setup(x => x.ScoreRepository.GetScoreIds(It.IsAny<List<int>>(), It.IsAny<List<int>>())).ReturnsAsync(existingScores);
            _unitOfWorkMock.Setup(x => x.ScoreRepository.UpdateRange(It.IsAny<List<Score>>())).Returns(Task.FromResult(true));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).Returns(Task.FromResult(1));

            // Act
            var result = await _scoreService.UpdateScoreByClassAsync(scoreModels);

            // Assert
            Assert.True(result.Status);
            Assert.Equal("Scores updated successfully", result.Message);
            _unitOfWorkMock.Verify(x => x.ScoreRepository.UpdateRange(It.IsAny<List<Score>>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }

        [Fact]
        public async Task DeleteScoreAsync_ScoreExists_DeletesScore()
        {
            // Arrange
            var studentId = 1;
            var classId = 1;
            var existingScore = new Score { StudentId = studentId, ClassId = classId };

            _unitOfWorkMock.Setup(x => x.ScoreRepository.GetScoreIdAsync(studentId, classId)).ReturnsAsync(existingScore);
            _unitOfWorkMock.Setup(x => x.ScoreRepository.SoftRemove(existingScore)).Returns(Task.FromResult(true));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            // Act
            var result = await _scoreService.DeleteScoreAsync(studentId, classId);

            // Assert
            Assert.True(result.Status);
            Assert.Equal("Score deleted successfully", result.Message);
            _unitOfWorkMock.Verify(x => x.ScoreRepository.SoftRemove(existingScore), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }

        [Fact]
        public async Task DeleteScoreAsync_ScoreDoesNotExist_ReturnsErrorMessage()
        {
            // Arrange
            var studentId = 1;
            var classId = 1;

            // Simulate a non-existing score
            _unitOfWorkMock.Setup(x => x.ScoreRepository.GetScoreIdAsync(studentId, classId)).ReturnsAsync((Score)null);

            // Act
            var result = await _scoreService.DeleteScoreAsync(studentId, classId);

            // Assert
            Assert.False(result.Status);
            Assert.Equal("Score is not existed!!", result.Message);
        }

        [Fact]
        public async Task DeleteScoresByClassAsync_ScoresExist_DeletesScores()
        {
            // Arrange
            var studentIds = new List<int> { 1, 2 };
            var classId = 1;
            var existingScores = new List<Score>
            {
                new Score { StudentId = 1, ClassId = classId },
                new Score { StudentId = 2, ClassId = classId }
            };

            _unitOfWorkMock.Setup(x => x.ScoreRepository.GetScoreIdsV2(studentIds, classId)).ReturnsAsync(existingScores);
            _unitOfWorkMock.Setup(x => x.ScoreRepository.SoftRemove(It.IsAny<Score>())).Returns(Task.FromResult(true));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            // Act
            var result = await _scoreService.DeleteScoresByClassAsync(studentIds, classId);

            // Assert
            Assert.True(result.Status);
            Assert.Equal("Scores delete successfully", result.Message);
            foreach (var score in existingScores)
            {
                _unitOfWorkMock.Verify(x => x.ScoreRepository.SoftRemove(score), Times.Once());
            }
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }

        [Fact]
        public async Task ScoreImportExcel_ScoresExist_AddSuccess()
        {
            // Arrange
            var scoreModels = new List<ScoreImportModel>
            {
                new ScoreImportModel { StudentCode = "S1", ClassId = 1 },
                new ScoreImportModel { StudentCode = "S2", ClassId = 2 }
            };
            var existingScores = new List<Score>
            {
                new Score { Student = new Student { StudentCode = "S1" }, ClassId = 1 },
                new Score { Student = new Student { StudentCode = "S2" }, ClassId = 2 }
            };
            var students = new List<Student>
            {
                new Student { Id = 1, StudentCode = "S1" },
                new Student { Id = 2, StudentCode = "S2" }
            };

            _unitOfWorkMock.Setup(x => x.ScoreRepository.GetScoreIdsV1(scoreModels)).ReturnsAsync(existingScores);
            _unitOfWorkMock.Setup(x => x.ScoreRepository.GetStudents(scoreModels)).ReturnsAsync(students);
            _unitOfWorkMock.Setup(x => x.ScoreRepository.AddRangeAsync(It.IsAny<List<Score>>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).Returns(Task.FromResult(1));

            // Act
            var result = await _scoreService.ScoreImportExcel(scoreModels);

            // Assert
            Assert.True(result.Status);
            Assert.Equal("Scores added successfully.", result.Message);
            _unitOfWorkMock.Verify(x => x.ScoreRepository.AddRangeAsync(It.IsAny<List<Score>>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
        }

    }
}
