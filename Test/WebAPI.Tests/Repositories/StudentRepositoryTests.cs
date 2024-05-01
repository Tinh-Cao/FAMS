using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Repositories;
using FAMS_GROUP2.Repositories.ViewModels.StudentModels;
using FluentAssertions;


namespace WebAPI.Tests.Repositories
{
    public class StudentRepositoryTests : SetupTest
    {
        private readonly IStudentRepository _studentRepository;


        public StudentRepositoryTests()
        {

            _studentRepository = new StudentRepository(_dbContext, _currentTimeMock.Object, _claimsServiceMock.Object, _configurationMock.Object);                            
        }

        [Fact]
        public async Task AddRangeStudentAsync_Should_ReturnCorrectData()
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
                .CreateMany(10).ToList();
            await _dbContext.Students.AddRangeAsync(mockData);
            // act
            var saveChanges = await _dbContext.SaveChangesAsync();
            var result = await _studentRepository.GetAllAsync();
            // assert
            saveChanges.Should().Be(mockData.Count());  
            result.Should().BeEquivalentTo(mockData);
        }

        [Fact]
        public async Task GetStudentDetails_ExistingStudent_ShouldReturnCorrectData()
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
                .Create();
            // Act
            var addedStudent = await _studentRepository.AddAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var result = await _studentRepository.GetByIdAsync(addedStudent.Id);
            // Assert
            result.Should().BeEquivalentTo(addedStudent);
        }

        [Fact]
        public async Task DeleteRangeExistingStudent_ShouldReturnCorrectData()
        {
            //ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Tạo dữ liệu mock
            var mockData = _fixture.Build<Student>()
                .Without(s => s.StudentCertificates)
                .Without(s => s.EmailSendStudents)
                .Without(s => s.Scores)
                .Without(s => s.StudentClasses)
                .CreateMany(5).ToList();
            // Act
            await _studentRepository.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var result = _studentRepository.DeleteRangeStudentAsync(mockData);
            var saveChanges = await _dbContext.SaveChangesAsync();
            // Assert
            result.Should().BeEquivalentTo(mockData);
            result.Should().AllSatisfy(b => b.IsDelete.Should().BeTrue());
            saveChanges.Should().Be(5);
        }

        [Fact]
        public async Task GetStudentByFilters_ShouldReturnCorrectData()
        {
            //ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Tạo dữ liệu mock
            var students = _fixture.Build<Student>()
                .Without(s => s.StudentCertificates)
                .Without(s => s.EmailSendStudents)
                .Without(s => s.Scores)
                .Without(s => s.StudentClasses)
                .CreateMany(10).ToList();
            var paginationParameter = new PaginationParameter();
            var studentFilterModel = new StudentFilterModel();
            var expectedResult = new Pagination<Student>(students, 10, 1, 1);

            // Act
            await _studentRepository.AddRangeAsync(students);
            var savechange = await _dbContext.SaveChangesAsync();
            var result = await _studentRepository.GetStudentsByFiltersAsync(paginationParameter, studentFilterModel);
            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task UnbanRangeExistingStudent_ShouldReturnCorrectData()
        {
            //ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Tạo dữ liệu mock
            var mockData = _fixture.Build<Student>()
                .With(s => s.IsDelete,true) // set mặc định bằng trạng thái đã xóa
                .Without(s => s.StudentCertificates)
                .Without(s => s.EmailSendStudents)
                .Without(s => s.Scores)
                .Without(s => s.StudentClasses)
                .CreateMany(5).ToList();
          //  mockData.ForEach(student => student.IsDelete = true); một cách viết khác xịn hơn
            // Act
            await _studentRepository.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var result = _studentRepository.UnbanRangeStudentAsync(mockData);
            var saveChanges = await _dbContext.SaveChangesAsync();
            // Assert
            result.Should().BeEquivalentTo(mockData);
            result.Should().AllSatisfy(b => b.IsDelete.Should().BeFalse());
            saveChanges.Should().Be(5);
        }




    }
}
