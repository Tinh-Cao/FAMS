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

namespace WebAPI.Tests.Repositories
{
    public class StudentClassRepositoryTests : SetupTest
    {
        private readonly IStudentClassRepository _studentClassRepository;

        public StudentClassRepositoryTests()
        {
            _studentClassRepository = new StudentClassRepository(_dbContext, _currentTimeMock.Object, _claimsServiceMock.Object);
        }

        [Fact]
        public async Task AddStudentToClass_ShouldReturnCorrectData()
        {
            //ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Tạo dữ liệu mock
            var mockClass = _fixture.Build<Class>()
                .Without(c => c.Scores)
                .Without(c => c.StudentClasses)
                .Without(c => c.Program)
                .Without(c => c.ClassAccounts)
                .Create();
            var mockStudent = _fixture.Build<Student>()
              .Without(s => s.StudentCertificates)
              .Without(s => s.EmailSendStudents)
              .Without(s => s.Scores)
              .Without(s => s.StudentClasses)
              .Create();
            StudentClass studentClass = new()
            {
                ClassId = mockClass.Id,
                StudentId = mockStudent.Id,
            };
            await _dbContext.Classes.AddAsync(mockClass);
            await _dbContext.Students.AddAsync(mockStudent);
            _dbContext.SaveChanges();
            // act
            await _studentClassRepository.AddAsync(studentClass);
            var result = await _dbContext.SaveChangesAsync();
            var checkResult = await _studentClassRepository.GetAllAsync();
            //Assert
            result.Should().Be(1);
            checkResult.First().Should().BeEquivalentTo(studentClass);
        }

        [Fact]
        public async Task GetStudentsFromClass_ShouldReturnCorrectData()
        {
            //ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Tạo dữ liệu mock
            var mockClass = _fixture.Build<Class>()
                .Without(c => c.Scores)
                .Without(c => c.StudentClasses)
                .Without(c => c.Program)
                .Without(c => c.ClassAccounts)
                .Create();
            var mockStudent = _fixture.Build<Student>()
              .Without(s => s.StudentCertificates)
              .Without(s => s.EmailSendStudents)
              .Without(s => s.Scores)
              .Without(s => s.StudentClasses)
              .CreateMany(5).ToList();

            var mockStudentClasses = mockStudent.Select(s => new StudentClass
            {
                StudentId = s.Id,
                ClassId=mockClass.Id
            }).ToList();
            

            await _dbContext.Classes.AddAsync(mockClass);
            await _dbContext.Students.AddRangeAsync(mockStudent);
            _dbContext.SaveChanges();
            await _studentClassRepository.AddRangeAsync(mockStudentClasses);
            var check = await _dbContext.SaveChangesAsync();
            // act
            var result = await _studentClassRepository.getStudentFromAClass(mockClass.Id);

            //Assert
            check.Should().Be(5);
            result.Count.Should().Be(5);
            result.Should().BeEquivalentTo(mockStudent, option => option.WithoutStrictOrdering());
        }


        [Fact]
        public async Task GetStudentsFromClass_ShouldReturnEmptyData()
        {
            //ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Tạo dữ liệu mock
            var mockClass = _fixture.Build<Class>()
                .Without(c => c.Scores)
                .Without(c => c.StudentClasses)
                .Without(c => c.Program)
                .Without(c => c.ClassAccounts)
                .Create();

            await _dbContext.Classes.AddAsync(mockClass);
            _dbContext.SaveChanges();

            var mockStudentClasses = _fixture.Build<StudentClass>()
                .Without(c => c.AttendanceClasses)
                .Without(c => c.Student)
                .Without( c=> c.StudentId)
                .With( c=> c.ClassId,mockClass.Id)
                .With( c=> c.Class,mockClass)
                .CreateMany(5).ToList();
         
            await _studentClassRepository.AddRangeAsync(mockStudentClasses);
            var check = await _dbContext.SaveChangesAsync();
            // act
            var result = await _studentClassRepository.getStudentFromAClass(mockClass.Id);

            //Assert
            check.Should().Be(5);
            result.Count.Should().Be(0);
        }


        [Fact]
        public async Task GetNumberStudentsOfClass_ShouldReturnCorrectData()
        {
            //ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Tạo dữ liệu mock
            var mockClass = _fixture.Build<Class>()
                .Without(c => c.Scores)
                .Without(c => c.StudentClasses)
                .Without(c => c.Program)
                .Without(c => c.ClassAccounts)
                .Create();
            var mockStudent = _fixture.Build<Student>()
              .Without(s => s.StudentCertificates)
              .Without(s => s.EmailSendStudents)
              .Without(s => s.Scores)
              .Without(s => s.StudentClasses)
              .CreateMany(5).ToList();

            var mockStudentClasses = new List<StudentClass>();
            foreach (var student in mockStudent)
            {
                mockStudentClasses.Add(
                    new StudentClass
                    {
                        StudentId = student.Id,
                        ClassId = mockClass.Id
                    }
                    );
            }


            await _dbContext.Classes.AddAsync(mockClass);
            await _dbContext.Students.AddRangeAsync(mockStudent);
            _dbContext.SaveChanges();
            await _studentClassRepository.AddRangeAsync(mockStudentClasses);
            var check = await _dbContext.SaveChangesAsync();
            // act
            var result = await _studentClassRepository.GetNumberOfStudent(mockClass.Id);

            //Assert
            check.Should().Be(5);
            result.Should().Be(5);
        }
    }
}
