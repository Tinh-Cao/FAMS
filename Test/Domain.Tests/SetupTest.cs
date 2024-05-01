using FAMS_GROUP2;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Services;
using AutoFixture;
using AutoMapper;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Repositories.Mapper;
using FAMS_GROUP2.Repositories.Entities;
using Castle.Core.Configuration;
using Microsoft.Extensions.Configuration;
using FAMS_GROUP2.Services.Services;
using FAMS_GROUP2.Repositories.ViewModels.EmailModel;
using Microsoft.Extensions.Options;

namespace Domain.Tests
{
    public class SetupTest : IDisposable
    {
        protected readonly IMapper _mapperConfig;
        protected readonly Fixture _fixture;
        protected readonly FamsDbContext _dbContext;

        // setup for service
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
        protected readonly Mock<IClaimsService> _claimsServiceMock;
        protected readonly Mock<ICurrentTime> _currentTimeMock;
        protected readonly Mock<Microsoft.Extensions.Configuration.IConfiguration> _configurationMock;

        protected readonly Mock<IUserManager> _userManagerMock;
        protected readonly Mock<IAccountService> _accountServiceMock;
        protected readonly Mock<IAttendanceClassService> _attendanceClassServiceMock;
        protected readonly Mock<IStudentService> _studentServiceMock;
        protected readonly Mock<ICertificateService> _certificateServiceMock;
        protected readonly Mock<IAssignmentService> _assignmentServiceMock;
        //protected readonly Mock<IAttendanceClassService> _attendanceServiceMock;
        protected readonly Mock<IClassService> _classServiceMock;
        protected readonly Mock<IDocumentService> _documentServiceMock;
        protected readonly Mock<IEmailSendService> _emailSendServiceMock;
        protected readonly Mock<IEmailTemplateServices> _emailTemplateServiceMock;
        protected readonly Mock<IScoreService> _scoreServiceMock;
        protected readonly Mock<IModuleService> _moduleServiceMock;
        protected readonly Mock<IMailService> _mailServiceMock;

        protected readonly Mock<ILessonService> _lessonServiceMock;
        protected readonly Mock<IProgramService> _programServiceMock;
        // setup for repository
        protected readonly Mock<IAccountRepository> _accountRepositoryMock;
        protected readonly Mock<IAttendanceClassRepository> _attendanceClassRepositoryMock;
        protected readonly Mock<IAssignmentRepository> _assignmentRepositoryMock;
        //protected readonly Mock<IAttendanceClassRepository> _attendanceClassRepositoryMock;
        //protected readonly Mock<IClassRepository> _classRepositoryMock;
        protected readonly Mock<IDocumentRepository> _documentRepositoryMock;
        //protected readonly Mock<IEmailSendRepository> _emailSendRepositoryMock;
        //protected readonly Mock<IEmailSendStudentRepository> _emailSendStudentRepositoryMock;
        //protected readonly Mock<IEmailTemplateRepository> _emailTemplateRepositoryMock;
        protected readonly Mock<ICertificateRepository> _certificateRepositoryMock;
        protected readonly Mock<IEmailTemplateRepository> _emailTemplateRepositoryMock;
        //protected readonly Mock<ICertificateRepository> _certificateRepositoryMock;
        protected readonly Mock<ILessonRepository> _lessonRepositoryMock;
        protected readonly Mock<IModuleRepository> _moduleRepositoryMock;
        //protected readonly Mock<IProgramModuleRepository> _programModuleRepositoryMock;
        protected readonly Mock<IProgramRepository> _programRepositoryMock;
        protected readonly Mock<IScoreRepository> _scoreRepositoryMock;
        protected readonly Mock<IStudentClassRepository> _studentClassRepositoryMock;
        protected readonly Mock<IStudentRepository> _studentRepositoryMock;


        public SetupTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperConfigProfile());
            });
            _mapperConfig = mappingConfig.CreateMapper();
            _fixture = new Fixture();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _currentTimeMock = new Mock<ICurrentTime>();
            _configurationMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            //Services
            _accountServiceMock = new Mock<IAccountService>();
            _attendanceClassServiceMock = new Mock<IAttendanceClassService>();
            _scoreServiceMock = new Mock<IScoreService>();
            _studentServiceMock = new Mock<IStudentService>();
            _claimsServiceMock = new Mock<IClaimsService>();
            _certificateServiceMock = new Mock<ICertificateService>();
            _moduleServiceMock = new Mock<IModuleService>();
            _classServiceMock = new Mock<IClassService>();
            _emailTemplateServiceMock = new Mock<IEmailTemplateServices>();
            _programServiceMock = new Mock<IProgramService>();
            _assignmentServiceMock = new Mock<IAssignmentService>();
            _lessonServiceMock = new Mock<ILessonService>();
            _documentServiceMock = new Mock<IDocumentService>();
            _mailServiceMock = new Mock<IMailService>();
            _documentServiceMock = new Mock<IDocumentService>();    
            //Repositories
            _userManagerMock = new Mock<IUserManager>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _attendanceClassRepositoryMock = new Mock<IAttendanceClassRepository>();
            _scoreRepositoryMock = new Mock<IScoreRepository>();
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _certificateRepositoryMock = new Mock<ICertificateRepository>();
            _emailTemplateRepositoryMock = new Mock<IEmailTemplateRepository>();
            _programRepositoryMock = new Mock<IProgramRepository>();
            _studentClassRepositoryMock = new Mock<IStudentClassRepository>();
            _documentRepositoryMock = new Mock<IDocumentRepository>();
            _moduleRepositoryMock = new Mock<IModuleRepository>();
            _assignmentRepositoryMock = new Mock<IAssignmentRepository>();
           
            var options = new DbContextOptionsBuilder<FamsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new FamsDbContext(options);

            _currentTimeMock.Setup(x => x.GetCurrentTime()).Returns(DateTime.UtcNow);
            _claimsServiceMock.Setup(x => x.GetCurrentUserId).Returns(0);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
