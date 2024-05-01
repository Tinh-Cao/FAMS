using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Repositories;
using FAMS_GROUP2.Repositories.ViewModels.CertificateModels;
using FluentAssertions;
using Newtonsoft.Json;

namespace WebAPI.Tests.Repositories
{
    public class CertificateRepositoryTests : SetupTest
    {
        private readonly ICertificateRepository _certificateRepository;
        public CertificateRepositoryTests()
        {
            _certificateRepository = new CertificateRepository(_dbContext, _currentTimeMock.Object, _claimsServiceMock.Object);
        }

        [Fact]
        public async Task CheckStudentCertificate_NotAddedBefore_ShouldReturnFalseResult()
        {
            //ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Tạo dữ liệu mock
            var mockStudentData = _fixture.Build<Student>()
               .Without(s => s.StudentCertificates)
               .Without(s => s.EmailSendStudents)
               .Without(s => s.Scores)
               .Without(s => s.StudentClasses)
               .CreateMany(3).ToList();

            var mockClassData = _fixture.Build<Class>()
               .Without(c => c.ClassAccounts)
               .Without(c => c.ProgramId)
               .Without(c => c.Scores)
               .Without(c => c.Program)
               .Without(c => c.StudentClasses)
               .Create();

            await _dbContext.Classes.AddAsync(mockClassData);
            await _dbContext.Students.AddRangeAsync(mockStudentData);
            await _dbContext.SaveChangesAsync();

            var mockTrainingProgram = _fixture.Build<TrainingProgram>()
             .Without(t => t.ProgramModules)
             .With(t => t.Classes, new List<Class> { mockClassData })
             .CreateMany(1).ToList();

            var mockStudentClasses = new List<StudentClass>();
            foreach (var student in mockStudentData)
            {
                mockStudentClasses.Add(new StudentClass { StudentId = student.Id, ClassId = mockClassData.Id });
            }
            await _dbContext.TrainingPrograms.AddRangeAsync(mockTrainingProgram);
            await _dbContext.StudentClasses.AddRangeAsync(mockStudentClasses);
            await _dbContext.SaveChangesAsync();
            // Act
            var result = await _certificateRepository.CheckCertificateByStudentAndClassAsync(mockStudentData.First().Id, mockClassData.Id);
            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task AddStudentCertificate_ShouldReturnTrueResult()
        {
            //ARRANGE
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Tạo dữ liệu mock
            var mockStudentData = _fixture.Build<Student>()
               .Without(s => s.StudentCertificates)
               .Without(s => s.EmailSendStudents)
               .Without(s => s.Scores)
               .Without(s => s.StudentClasses)
               .CreateMany(1).ToList();

            var mockCertificateData = _fixture.Build<Certificate>()
               .Without(c => c.StudentCertificates)
               .With(c => c.IsDelete, false)
               .CreateMany(3).ToList();

            await _dbContext.Certificates.AddRangeAsync(mockCertificateData);
            await _dbContext.Students.AddRangeAsync(mockStudentData);
            await _dbContext.SaveChangesAsync();

            var certificateContent = _fixture.Build<CertificateContentModel>().Create();
            var contentJson = JsonConvert.SerializeObject(certificateContent);
            // Act
            var result = await _certificateRepository.AddCertificateToStudentAsync(mockStudentData.First().Id, mockCertificateData.First().Id, contentJson);
            // Assert
            result.Should().BeTrue();
        }

    }
}
