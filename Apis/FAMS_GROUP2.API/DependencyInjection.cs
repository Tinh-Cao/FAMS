using FAMS_GROUP2.API.Services;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.Mapper;
using FAMS_GROUP2.Repositories.Repositories;
using FAMS_GROUP2.Services;
using FAMS_GROUP2.Services.Interfaces;
using FAMS_GROUP2.Services.Services;
using System.Diagnostics;
using WebAPI.Middlewares;

namespace FAMS_GROUP2.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            // add DI
            services.AddScoped<ICurrentTime, CurrentTime>();

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudentService, StudentService>();


            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IGenericRepository<Class>, GenericRepository<Class>>();
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<IClassAccountRepository, ClassAccountRepository>();

            services.AddScoped<IGenericRepository<StudentClass>, GenericRepository<StudentClass>>();
            services.AddScoped<IStudentClassRepository, StudentClassRepository>();
            services.AddScoped<IStudentClassService, StudentClassService>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IModuleService, ModuleService>();

            services.AddScoped<IGenericRepository<AttendanceClass>, GenericRepository<AttendanceClass>>();
            services.AddScoped<IAttendanceClassRepository, AttendanceClassRepository>();
            services.AddScoped<IAttendanceClassService, AttendanceClassService>();

         
            services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
            services.AddScoped<IEmailTemplateServices, EmailTemplateServices>();

            services.AddAutoMapper(typeof(MapperConfigProfile).Assembly);

            services.AddSingleton<GlobalExceptionMiddleware>();
            services.AddSingleton<PerformanceMiddleware>();
            services.AddSingleton<Stopwatch>();

            services.AddScoped<IClaimsService, ClaimsService>();

            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IClassService, ClassService>();

            services.AddScoped<IProgramRepository, ProgramRepository>();
            services.AddScoped<IProgramService, ProgramService>();

            services.AddScoped<IProgramModuleRepository, ProgramModuleRepository>();

            services.AddScoped<IUserManager, UserManager>();

            services.AddScoped<IScoreRepository, ScoreRepository>();
            services.AddScoped<IScoreService, ScoreService>();

            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<IAssignmentService, AssignmentService>();

            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<ILessonService, LessonService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();



            services.AddScoped<IEmailSendRepository, EmailSendRepository>();
            services.AddScoped<IEmailSendService, EmailSendService>();

            services.AddScoped<IEmailSendStudentRepository, EmailSendStudentRepository>();
           

            services.AddTransient<IMailService, MailService>();
            services.AddHttpContextAccessor();

            services.AddScoped<ICertificateRepository, CertificateRepository>();
            services.AddScoped<ICertificateService, CertificateService>();

            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IDocumentService, DocumentService>();    

            
            return services;
        }
    }
}
