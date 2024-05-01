using AutoFixture;
using AutoMapper;
using Domain.Tests;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.ViewModels.EmailModel;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Services.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace WebAPI.Tests.Services
{
    public  class MailServiceTests : SetupTest
    {
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly IOptions<MailSettings> config;

        public MailServiceTests()
        {
            _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();

            var mailSettings = new MailSettings
            {
                Mail = _configuration["MailSettings:Mail"],
                DisplayName = _configuration["MailSettings:DisplayName"],
                Password = _configuration["MailSettings:Password"],
                Host = _configuration["MailSettings:Host"],
                Port = _configuration["MailSettings:Port"]
            };

            var options = Options.Create(mailSettings);

            _mailService = new MailService(options, _mapperConfig, _unitOfWorkMock.Object);

        }

        [Fact]
        public async Task SendEmailAsync_SuccessfullySendsEmail()
        {

            var receiveMail = _fixture.Build<EmailSendStudent>()
                .Without(x => x.EmailSend)
                .Without(x => x.Receive).Create();
            // Arrange
            var mailRequest = new MailRequest
            {
                StudentIds = new List<int> { 1, 2, 3 },
                Body = "Test Body",
                Subject = "test",
                Attachments =  null
               
            };

            var students = new List<Student>
        {
            new Student { Id = 1, Email = "qdoan10122002@gmail.com" },
            new Student { Id = 2, Email = "qdoan10122002@gail.com" },
            new Student { Id = 3, Email = "qdoan10122002@gail.com" }
        };
            

            _unitOfWorkMock.Setup(uow => uow.EmailSendRepository.CheckId(mailRequest.StudentIds)).ReturnsAsync(students);
            _unitOfWorkMock.Setup(uow => uow.SaveChangeAsync()).ReturnsAsync(1);
        
            _unitOfWorkMock.Setup(x => x.EmailSendStudentRepository.AddAsync(receiveMail)).ReturnsAsync(receiveMail);
            // Act
            var result = await _mailService.SendEmailAsync(mailRequest);

            // Assert
            Assert.Equal("Send Mail Successfully!", result.Message);
           
        }

    }
}
