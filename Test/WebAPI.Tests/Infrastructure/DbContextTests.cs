using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Infrastructure
{
    public class DbContextTests : SetupTest
    {
        private readonly IGenericRepository<Student> _studentRepository;

        public DbContextTests()
        {
            _studentRepository = new GenericRepository<Student>(
                _dbContext,
                _currentTimeMock.Object,
                _claimsServiceMock.Object
                );
        }


     //   [Fact]
        public async Task FamsDbContext_FamsDbSetShouldReturnCorrectDataa()
        {
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

            await _studentRepository.AddRangeAsync( mockData );
            await _dbContext.SaveChangesAsync();

            var result = await _dbContext.Students.ToListAsync();
            result.Should().BeEquivalentTo(mockData);
        }

        [Fact]
        public async Task FamsDbContext_FamsDbSetShouldReturnCorrectData()
        {
            //ARRANGE
            // Tạo Customization cho lớp Student xử lý lỗi CircularReference
            _fixture.Customize<Student>(composer =>
                composer
                    .Without(s => s.StudentCertificates)
                    .Without(s => s.EmailSendStudents)
                    .Without(s => s.Scores)
                    .Without(s => s.StudentClasses)
                    .OmitAutoProperties());

            // Tạo dữ liệu mock
            var mockData = _fixture.CreateMany<Student>(10).ToList();
            //ACT
            await _dbContext.Students.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var result = await _dbContext.Students.ToListAsync();
            //ASSERT
            result.Should().BeEquivalentTo(mockData);
            // result.Should().BeEquivalentTo(mockData, options => options.ExcludingMissingMembers()); bỏ qua so sánh thuộc tính loại bỏ
            
        }

        [Fact]
        public async Task FamsDbContext_FamsDbSetShouldReturnEmptylistWhenNotHavingData()
        {
            var result = await _dbContext.Students.ToListAsync();
            result.Should().BeEmpty();
        }

    }
}
