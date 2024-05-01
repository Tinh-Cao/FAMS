using AutoFixture;
using AutoFixture.Kernel;
using Domain.Tests;
using FAMS_GROUP2.API.Controllers;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.AttendanceModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Controllers
{
    public class AttendanceControllerTest : SetupTest
    {
        private AttendanceClassController _attendanceClassController;
        private readonly Fixture _fixture;
        public AttendanceControllerTest()
        {
            _fixture = new Fixture();
            _attendanceClassController = new AttendanceClassController(_attendanceClassServiceMock.Object);
        }
        [Fact]
        public async Task AddAttendance_ShouldReturnSuccess()
        {
            var mockModelRequest = _fixture.CreateMany<AttendanceClassViewOfClassModel>(10).ToList();
            var mockModelResponse = _fixture.Build<AttendanceClassResultModel>().Create();
            _attendanceClassServiceMock.Setup(x => x.AddAttendanceOfClass(mockModelRequest, null, true)).ReturnsAsync(mockModelResponse);
            var result = await _attendanceClassController.AddAttendanceClass(mockModelRequest);
            _attendanceClassServiceMock.Verify(x => x.AddAttendanceOfClass(mockModelRequest,null,true), Times.Once);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task AddListAttendances_ShouldReturnSuccess()
        {
            AttendanceClassToAddListModel mockModelRequest = _fixture.Build<AttendanceClassToAddListModel>()
                .With(x=>x.FromDate,new DateTime(2024,03,01))
                .With(x => x.ToDate, new DateTime(2024, 05, 01))
                .Create();
            var mockModelResponse = _fixture.Build<AttendanceClassResultModel>().Create();
            _attendanceClassServiceMock.Setup(x => x.AddListAttendanceOfClassByDate(mockModelRequest)).ReturnsAsync(mockModelResponse);
            var result = await _attendanceClassController.AddListAttendanceOfClassByDate(mockModelRequest);
            
            _attendanceClassServiceMock.Verify(x => x.AddListAttendanceOfClassByDate(mockModelRequest), Times.Once);
            if (mockModelResponse.status == true)
            {
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                okResult.StatusCode.Should().Be(200);
            }
        }

        [Fact]
        public async Task UpdateDateAttendanceClass_ShouldReturnSuccess()
        {
            AttendanceClassDateUpdateModel mockModelRequest = _fixture.Build<AttendanceClassDateUpdateModel>()
                .With(x => x.PreviousDate, new DateTime(2024, 03, 24))
                .With(x => x.AfterDate, new DateTime(2024, 05, 24))
                .Create();
            var mockModelResponse = _fixture.Build<AttendanceClassResultModel>().Create();
            _attendanceClassServiceMock.Setup(x => x.UpdateAttendanceClass(mockModelRequest)).ReturnsAsync(mockModelResponse);
            var result = await _attendanceClassController.UpdateDateAttendanceClass(mockModelRequest);

            _attendanceClassServiceMock.Verify(x => x.UpdateAttendanceClass(mockModelRequest), Times.Once);
            if (mockModelResponse.status == true)
            {
                var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
                okResult.StatusCode.Should().Be(200);
            }
        }

    }
}
