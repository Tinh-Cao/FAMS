using AutoFixture;
using Domain.Tests;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Repositories;
using FAMS_GROUP2.Repositories.ViewModels.ProgramModels;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Repositories
{
    public class ProgramRepositotyTests : SetupTest
    {
        private readonly IProgramRepository _programRepository;


        public ProgramRepositotyTests()
        {

            _programRepository = new ProgramRepository(_dbContext,_currentTimeMock.Object, _claimsServiceMock.Object);
        }
        [Fact]
       
        public async Task GetTrainingProgramsByFiltersAsync_ShouldReturnPaginatedPrograms()
        {
            // Arrange
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var mockTrainingPrograms = _fixture.Build<TrainingProgram>()
                .With(tp => tp.IsDelete, false)
                .Without(tp => tp.ProgramModules)
                .CreateMany(2)
                .ToList();
            _dbContext.TrainingPrograms.AddRange(mockTrainingPrograms);
            await _dbContext.SaveChangesAsync();

            var paginationParameter = new PaginationParameter
            {
                PageIndex = 1,
                PageSize = 10
            };

            var programFilterModel = _fixture.Build<ProgramFilterModel>()
                                             .OmitAutoProperties()
                                             .Without(x => x.Search)
                                             .Without(x=>x.Duration)
                                             .Without(x=>x.isDelete)
                                             .Without(x => x.Status)
                                             .Create();
            
            // Act
            var result = await _programRepository.GetTrainingProgramsByFiltersAsync(paginationParameter,programFilterModel);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2); 
            result.TotalCount.Should().Be(2); 

        }
        [Fact]

        public async Task GetTrainingProgramsByFiltersAsync_ShouldReturnEmptyList()
        {
            // Arrange
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var paginationParameter = new PaginationParameter
            {
                PageIndex = 1,
                PageSize = 10
            };

            var programFilterModel = _fixture.Build<ProgramFilterModel>()
                                             .OmitAutoProperties()
                                             .Without(x => x.Search)
                                             .Without(x => x.Duration)
                                             .Without(x => x.isDelete)
                                             .Without(x => x.Status)
                                             .Create();

            // Act
            var result = await _programRepository.GetTrainingProgramsByFiltersAsync(paginationParameter, programFilterModel);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
            result.TotalCount.Should().Be(0);

        }
        [Fact]
        public async Task GetTrainingPrograms_ShouldReturnCorrectData()
        {

            // Arrange
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var mockreturnTP = _fixture.Build<TrainingProgram>()
                                 .With(tp => tp.IsDelete,false)
                                 .With(tp => tp.Status,"Active")
                                 .Without(tp => tp.ProgramModules)
                                 .CreateMany(5)
                                 .ToList();
            _dbContext.TrainingPrograms.AddRange(mockreturnTP);
           
            var mockTP = _fixture.Build<TrainingProgram>()
                                .With(tp => tp.IsDelete, true)
                                .With(tp => tp.Status, "Stop")
                                .Without(tp => tp.ProgramModules)
                                .CreateMany(2)
                                .ToList();
            _dbContext.TrainingPrograms.AddRange(mockTP);
            await _dbContext.SaveChangesAsync();
            // Act
            var result = await _programRepository.GetTrainingPrograms();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            
        }
        public async Task GetTrainingProgramById_ShouldReturnCorrectId()
        {
            // Arrange
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var tpId = 1;
            var mockTP = _fixture.Build<TrainingProgram>()
                                 .With(tp => tp.IsDelete, false)
                                 .With(tp => tp.Status, "Active")
                                 .Without(tp => tp.ProgramModules)
                                 .Create();
            _dbContext.TrainingPrograms.Add(mockTP);
            await _dbContext.SaveChangesAsync();

            //Act
            var result = await _programRepository.GetTrainingProgramById(tpId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(mockTP);

                                

        }
        [Fact]
        public async Task GetTrainingProgramById_ShouldReturnNull()
        {
            // Arrange
            // Xử lý circular reference bằng OmitOnRecursionBehavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var tpId = 2;
            var mockTP = _fixture.Build<TrainingProgram>()
                                 .With(tp => tp.IsDelete, false)
                                 .With(tp => tp.Status, "Active")
                                 .Without(tp => tp.ProgramModules)
                                 .Create();
            _dbContext.TrainingPrograms.Add(mockTP);
            await _dbContext.SaveChangesAsync();

            //Act
            var result = await _programRepository.GetTrainingProgramById(tpId);

            //Assert
            result.Should().BeNull();
           // result.Should().BeEquivalentTo(mockTP);



        }
    }

}

    



