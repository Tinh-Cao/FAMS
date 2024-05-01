using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.StudentModels;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Repositories
{
    public class ClassRepositoryTests:SetupTest
    {
        private readonly IClassRepository _classRepository;
        public ClassRepositoryTests()
        {
            _classRepository = new ClassRepository(_dbContext, _currentTimeMock.Object, _claimsServiceMock.Object);
        }

        [Fact]
        public async Task AddRangeClassAsync_Should_ReturnCorrectData()
        {
            //ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Tạo dữ liệu mock
            // var mockData = _fixture.Build<Student>().CreateMany(10).ToList();
            var mockData = _fixture.Build<Class>()
                .Without(c => c.Scores)
                .Without(c => c.StudentClasses)
                .Without(c => c.Program)
                .Without(c => c.ClassAccounts)
                .CreateMany(5).ToList();
            // act
            await _classRepository.AddRangeAsync(mockData);
            var saveChanges = await _dbContext.SaveChangesAsync();
            var result = await _classRepository.GetAllAsync();
            // assert
            saveChanges.Should().Be(mockData.Count());
            result.Should().BeEquivalentTo(mockData);
        }

        [Fact]
        public async Task GetClassByFilters_ShouldReturnCorrectData()
        {
            //ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Tạo dữ liệu mock
            var mockData = _fixture.Build<Class>()
                .With(c => c.IsDelete , false)
              .Without(c => c.Scores)
              .Without(c => c.StudentClasses)
              .Without(c => c.Program)
              .Without(c => c.ClassAccounts)
              .CreateMany(5).ToList();
            var paginationParameter = new PaginationParameter();
            var studentFilterModel = new ClassesFilterModel();
            var expectedResult = new Pagination<Class>(mockData, 5, 1, 1);

            // Act
            await _classRepository.AddRangeAsync(mockData);
            var savechange = await _dbContext.SaveChangesAsync();
            var result = await _classRepository.GetClassesByFiltersAsync(paginationParameter,studentFilterModel);
            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
