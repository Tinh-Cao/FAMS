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
    public class GenericRepositoryTests : SetupTest
    {
        private readonly IGenericRepository<Student> _genericRepository;

        public GenericRepositoryTests()
        {
            _genericRepository = new GenericRepository<Student>(_dbContext, _currentTimeMock.Object, _claimsServiceMock.Object);
        }

        [Fact]
        public async Task GenericRepository_GetAllAsync_ShouldReturnCorrectData()
        {
            //ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Tạo dữ liệu mock
            // var mockData = _fixture.Build<Student>().CreateMany(10).ToList();
            var mockData = _fixture.Build<Student>()
                .Without(s => s.StudentCertificates)
                .Without(s => s.EmailSendStudents)
                .Without(s => s.Scores)
                .Without(s => s.StudentClasses)
                .CreateMany(10)
                .ToList();
            //ACT
            await _dbContext.Students.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            //ASSERT
            var result = await _genericRepository.GetAllAsync();
            result.Should().BeEquivalentTo(mockData);
        }

        [Fact]
        public async Task GenericRepository_GetAllAsync_ShouldReturnEmptyWhenHaveNoData()
        {

            var result = await _genericRepository.GetAllAsync();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GenericRepository_GetByIdAsync_ShouldReturnCorrectData()
        {
            //ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Tạo dữ liệu mock
            // var mockData = _fixture.Build<Student>().CreateMany(10).ToList();
            var mockData = _fixture.Build<Student>()
                .Without(s => s.StudentCertificates)
                .Without(s => s.EmailSendStudents)
                .Without(s => s.Scores)
                .Without(s => s.StudentClasses)
                .CreateMany(1).ToList();
            //ACT
            await _dbContext.Students.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var result = await _genericRepository.GetByIdAsync(mockData.First().Id);
            //ASSERT
            result.Should().BeEquivalentTo(mockData.First());
        }

        [Fact]
        public async Task GenericRepository_AddAsync_ShouldReturnCorrectData()
        {
            //ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Tạo dữ liệu mock
            // var mockData = _fixture.Build<Student>().CreateMany(10).ToList();
            var mockData = _fixture.Build<Student>()
                .Without(s => s.StudentCertificates)
                .Without(s => s.EmailSendStudents)
                .Without(s => s.Scores)
                .Without(s => s.StudentClasses)
                .CreateMany(1).ToList();

            //ACT
            var result = await _genericRepository.AddAsync(mockData.First());
            var savechanges = await _dbContext.SaveChangesAsync();

            savechanges.Should().Be(1);
            result.Should().BeEquivalentTo(mockData.First());

        }

        [Fact]
        public async Task GenericRepository_Update_ShouldReturnCorrectData()
        {
            //ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Tạo dữ liệu mock
            // var mockData = _fixture.Build<Student>().CreateMany(10).ToList();
            var mockData = _fixture.Build<Student>()
                .Without(s => s.StudentCertificates)
                .Without(s => s.EmailSendStudents)
                .Without(s => s.Scores)
                .Without(s => s.StudentClasses)
                .CreateMany(1).ToList();

            //ACT
            _dbContext.Students.Add(mockData.First());
            await _dbContext.SaveChangesAsync();


            await _genericRepository.Update(mockData.First());
            var result = await _dbContext.SaveChangesAsync();

            result.Should().Be(1);
        }


    }
}
