using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FluentAssertions;
using FAMS_GROUP2.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAMS_GROUP2.Repositories.ViewModels.ScoreModels;
using FAMS_GROUP2.API.Controllers;
using Moq;

namespace WebAPI.Tests.Repositories
{
    public class ScoreRepositoryTests:SetupTest
    {
        private readonly IScoreRepository _scoreRepository;
        public ScoreRepositoryTests()
        {            
            _scoreRepository = new ScoreRepository(_dbContext, _currentTimeMock.Object, _claimsServiceMock.Object);
        }

        [Fact]
        public async Task GetScoreByFilters_ShouldReturnCorrectData()
        {
            //ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Tạo dữ liệu mock
            var mockData = _fixture.Build<Score>()
                .With(c => c.IsDelete, false)
                .Without(c => c.Student)
                .Without(c => c.Class)
                .CreateMany(5).ToList();
            var paginationParameter = new PaginationParameter();
            var studentFilterModel = new ScoreFilterModel();
            var expectedResult = new Pagination<Score>(mockData, 5, 1, 1);

            // Act
            await _scoreRepository.AddRangeAsync(mockData);
            var savechange = await _dbContext.SaveChangesAsync();
            var result = await _scoreRepository.GetScoresByFiltersAsync(paginationParameter, studentFilterModel);
            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetScoreById_ExistingScore_ShouldReturnCorrectData()
        {
            // ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Tạo dữ liệu mock 
            var mockData = _fixture.Build<Score>()
                .Without(m => m.Student)
                .Without(m => m.Class)
                .Create();

            await _scoreRepository.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();

            // ACT
            if (mockData.StudentId.HasValue && mockData.ClassId.HasValue)
            {
                var result = await _scoreRepository.GetScoreIdAsync(mockData.StudentId.Value, mockData.ClassId.Value);

                // ASSERT
                result.Should().BeEquivalentTo(mockData);
            }
        }

        [Fact]
        public async Task GetScoreIds_ShouldReturnCorrectData()
        {
            // ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Tạo dữ liệu mock 
            var mockData = _fixture.Build<Score>()
                .Without(m => m.Student)
                .Without(m => m.Class)
                .CreateMany(5).ToList();

            await _scoreRepository.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();

            // Tạo danh sách StudentId và ClassId từ mockData
            var studentIds = mockData.Select(m => m.StudentId.Value).ToList();
            var classIds = mockData.Select(m => m.ClassId.Value).ToList();

            // ACT
            var result = await _scoreRepository.GetScoreIds(studentIds, classIds);

            // ASSERT
            result.Should().BeEquivalentTo(mockData);
        }

        [Fact]
        public async Task GetScoreIdsV1_ShouldReturnCorrectData()
        {
            // ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Tạo dữ liệu mock 
            var mockScores = _fixture.Build<Score>()
                .Without(m => m.Student)
                .Without(m => m.Class)
                .CreateMany(5).ToList();

            var mockScoreImportModels = mockScores.Select(s => new ScoreImportModel { StudentCode = s.Student?.StudentCode, ClassId = (int)s.ClassId }).ToList();

            await _scoreRepository.AddRangeAsync(mockScores);
            await _dbContext.SaveChangesAsync();

            // ACT
            var result = await _scoreRepository.GetScoreIdsV1(mockScoreImportModels);

            // ASSERT
            result.Should().BeEquivalentTo(mockScores);
        }

        [Fact]
        public async Task GetScoreIdsV2_ShouldReturnCorrectData()
        {
            // ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Tạo dữ liệu mock 
            var mockScores = _fixture.Build<Score>()
                .Without(m => m.Student)
                .Without(m => m.Class)
                .CreateMany(5).ToList();

            var mockStudentIds = mockScores.Select(s => s.StudentId.Value).ToList();
            var mockClassId = 1;

            await _scoreRepository.AddRangeAsync(mockScores);
            await _dbContext.SaveChangesAsync();

            // ACT
            var result = await _scoreRepository.GetScoreIdsV2(mockStudentIds, mockClassId);

            // ASSERT
            result.Should().BeEquivalentTo(mockScores.Where(s => s.ClassId == mockClassId));
        }

        [Fact]
        public async Task GetName_ShouldReturnCorrectData()
        {
            // ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Tạo dữ liệu mock 
            var mockData = _fixture.Build<Score>()
                .Without(m => m.Student)
                .Without(m => m.Class)
                .Create();

            var mockStudent = _fixture.Build<Student>()
                .With(s => s.Id, mockData.StudentId.Value)
                .Create();

            await _scoreRepository.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Students.AddAsync(mockStudent);
            await _dbContext.SaveChangesAsync();

            // ACT
            var result = await _scoreRepository.GetName(mockData.StudentId.Value);

            // ASSERT
            result.Should().BeEquivalentTo(mockStudent.FullName);
        }

        [Fact]
        public async Task GetStudents_ShouldReturnCorrectData()
        {
            // ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Tạo dữ liệu mock 
            var mockScores = _fixture.Build<Score>()
                .Without(m => m.Student)
                .Without(m => m.Class)
                .CreateMany(5).ToList();

            var mockScoreImportModels = mockScores.Select(s => new ScoreImportModel { StudentCode = s.Student?.StudentCode, ClassId = (int)s.ClassId }).ToList();

            var mockStudents = _fixture.Build<Student>()
                .With(s => s.StudentCode, mockScores.First().Student?.StudentCode)
                .CreateMany(5).ToList();

            await _scoreRepository.AddRangeAsync(mockScores);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Students.AddRangeAsync(mockStudents);
            await _dbContext.SaveChangesAsync();

            // ACT
            var result = await _scoreRepository.GetStudents(mockScoreImportModels);

            // ASSERT
            result.Should().BeEquivalentTo(mockStudents.Where(s => s.StudentCode == mockScores.First().Student?.StudentCode));
        }

    }
}
