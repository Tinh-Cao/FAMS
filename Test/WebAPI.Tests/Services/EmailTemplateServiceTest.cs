using AutoMapper;
using Domain.Tests;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.ViewModels.EmailModel;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Services.Services;
using Moq;
using NuGet.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Services
{
    public class EmailTemplateServiceTest : SetupTest
    {
        private EmailTemplateServices _emailTemplateServices;
        public EmailTemplateServiceTest()
        {
            _emailTemplateServices = new EmailTemplateServices(
               _emailTemplateRepositoryMock.Object,
               _unitOfWorkMock.Object,
               _mapperConfig);
        }

        [Fact]
        public async Task GetAllEmailTemplateAsync_ReturnsListOfEmailTemplates()
        {
            // Arrange
            var emailTemplates = new List<EmailTemplate>
            {
                new EmailTemplate { Id = 1, Name = "Template1" },
                new EmailTemplate { Id = 2, Name = "Template2" },
                new EmailTemplate { Id = 3, Name = "Template3" }
            };

            _unitOfWorkMock.Setup(uow => uow.EmailTemplateRepository.GetAllAsync()).ReturnsAsync(emailTemplates);

            // Act
            var result = await _emailTemplateServices.GetAllEmailTemplateAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(emailTemplates.Count, result.Count);
            Assert.Equal(emailTemplates.Select(et => et.Id), result.Select(et => et.Id));
          
        }
        [Fact]
        public async Task GetAllEmailTemplateByIdAsync_ReturnsEmailTemplateIfExists()
        {
            // Arrange
            int existingId = 1;
            var existingEmailTemplate = new EmailTemplate { Id = existingId, Name = "ExistingTemplate" };
            _unitOfWorkMock.Setup(uow => uow.EmailTemplateRepository.GetByIdAsync(existingId)).ReturnsAsync(existingEmailTemplate);

            // Act
            var result = await _emailTemplateServices.GetAllEmailTemplateByIdAsync(existingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingId, result.Id);
         
        }
        [Fact]
        public async Task GetAllEmailTemplateByIdAsync_ReturnsNullForNonExistingId()
        {
            // Arrange
            int nonExistingId = 100;
            _unitOfWorkMock.Setup(uow => uow.EmailTemplateRepository.GetByIdAsync(nonExistingId)).ReturnsAsync((EmailTemplate)null);

            // Act
            var result = await _emailTemplateServices.GetAllEmailTemplateByIdAsync(nonExistingId);

            // Assert
            Assert.Null(result);
          
        }
        [Fact]
        public async Task DeleteEmailTemplateAsync_DeletesTemplateIfNotInUseAndExists()
        {
            // Arrange
            int existingId = 1;
            var existingEmailTemplate = new EmailTemplate { Id = existingId, Name = "ExistingTemplate" };
            _unitOfWorkMock.Setup(uow => uow.EmailTemplateRepository.GetByIdAsync(existingId)).ReturnsAsync(existingEmailTemplate);
            _unitOfWorkMock.Setup(uow => uow.EmailSendRepository.CheckIdTemplate(existingId)).ReturnsAsync(new List<EmailSend>());

            // Act
            var result = await _emailTemplateServices.DeleteEmailTemplateAsync(existingId);

            // Assert
            Assert.True(result.Status);
            Assert.Equal("remove successfull.", result.Message);
           
        }


        [Fact]
        public async Task DeleteEmailTemplateAsync_ReturnsErrorIfTemplateIsInUse()
        {
            // Arrange
            int existingId = 1;
            var existingEmailTemplate = new EmailTemplate { Id = existingId, Name = "ExistingTemplate" };
            var emailsUsingTemplate = new List<EmailSend> { new EmailSend {  } };
            _unitOfWorkMock.Setup(uow => uow.EmailTemplateRepository.GetByIdAsync(existingId)).ReturnsAsync(existingEmailTemplate);
            _unitOfWorkMock.Setup(uow => uow.EmailSendRepository.CheckIdTemplate(existingId)).ReturnsAsync(emailsUsingTemplate);

            // Act
            var result = await _emailTemplateServices.DeleteEmailTemplateAsync(existingId);

            // Assert
            Assert.False(result.Status);
            Assert.Equal(" EmailTemplate using.", result.Message);
        
        }
        [Fact]
        public async Task AddEmailTemplateAsync_Should_Return_Template_When_Successful()
        {
            // Arrange
            var templateModel = new EmailTemplateModel();
            var emailTemplate = new EmailTemplate();

            _unitOfWorkMock.Setup(uow => uow.EmailTemplateRepository)
                .Returns(_emailTemplateRepositoryMock.Object);

            _emailTemplateRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<EmailTemplate>()))
                .ReturnsAsync(emailTemplate);

            // Act
            var result = await _emailTemplateServices.AddEmailTemplateAsync(templateModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(emailTemplate, result);
            _unitOfWorkMock.Verify(uow => uow.EmailTemplateRepository.AddAsync(It.IsAny<EmailTemplate>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangeAsync(), Times.Once);
        }

    }
}
