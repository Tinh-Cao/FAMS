using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        //register repositories
        private readonly FamsDbContext _famsDbContext;
        private readonly IStudentRepository _studentRepository;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IProgramModuleRepository _programModuleRepository;
        private readonly IProgramRepository _programRepository;
        private readonly IScoreRepository _scoreRepository;
        private readonly IUserManager _userManager;
        private readonly IClassRepository _classRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAttendanceClassRepository _attendanceClassRepository;
        private readonly IStudentClassRepository _studentClassRepository;
        private readonly IClassAccountRepository _classAccountRepository;      
        private readonly ICertificateRepository _certificateRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IEmailSendRepository _emailSendRepository;
        private readonly IEmailSendStudentRepository _emailSendStudentRepository;
        private readonly FamsDbContext _dbContext;


        //config
        private bool disposedValue;

        public UnitOfWork(FamsDbContext famsDbContext, IStudentRepository studentRepository,
            IAccountRepository accountRepository, IAssignmentRepository assignmentRepository,
            IClassRepository classRepository, IEmailTemplateRepository emailTemplateRepository,
            ILessonRepository lessonRepository, IModuleRepository moduleRepository,
            IProgramModuleRepository programModuleRepository, IProgramRepository programRepository,
            IAttendanceClassRepository attendanceClassRepository, IStudentClassRepository studentClassRepository,
            IScoreRepository scoreRepository, IUserManager userManager, ICertificateRepository certificateRepository,
            IDocumentRepository documentRepository, IEmailSendRepository emailSendRepository, IEmailSendStudentRepository emailSendStudentRepository,
            IClassAccountRepository classAccountRepository, FamsDbContext dbContext)
        {
          
            this._famsDbContext = famsDbContext;
            this._studentRepository = studentRepository;
            this._emailTemplateRepository = emailTemplateRepository;
            this._lessonRepository = lessonRepository;
            this._moduleRepository = moduleRepository;
            this._programModuleRepository = programModuleRepository;
            this._programRepository = programRepository;
            this._scoreRepository = scoreRepository;
            this._userManager = userManager;
            this._classRepository = classRepository;
            this._assignmentRepository = assignmentRepository;
            this._accountRepository = accountRepository;
            this._attendanceClassRepository = attendanceClassRepository;
            this._studentClassRepository = studentClassRepository;
            this._emailSendRepository = emailSendRepository;
            this._emailSendStudentRepository = emailSendStudentRepository;
            this._dbContext = dbContext;
            this._certificateRepository = certificateRepository;
            this._documentRepository = documentRepository;
            this._emailSendStudentRepository = emailSendStudentRepository;
            this._classAccountRepository = classAccountRepository;
        }

        public IAccountRepository AccountRepository { get { return _accountRepository; } }
        public IAssignmentRepository AssignmentRepository { get { return _assignmentRepository; } }
        public IClassRepository ClassRepository { get { return _classRepository; } }
        public IEmailTemplateRepository EmailTemplateRepository { get { return _emailTemplateRepository; } }
        public IScoreRepository ScoreRepository { get { return _scoreRepository; } }
        public IUserManager UserManager { get {  return _userManager; } }
        public ILessonRepository LessonRepository { get { return _lessonRepository; } }
        public IModuleRepository ModuleRepository { get { return _moduleRepository; } }
        public IProgramModuleRepository ProgramModuleRepository { get { return _programModuleRepository; } }
        public IProgramRepository ProgramRepository { get { return _programRepository; } }
        public IStudentRepository StudentRepository { get { return _studentRepository; } }
        public ICertificateRepository CertificateRepository { get { return _certificateRepository;} }
        public IClassAccountRepository ClassAccountRepository { get {return _classAccountRepository;} }

        public IAttendanceClassRepository AttendanceClassRepository { get { return _attendanceClassRepository;} }
        public IStudentClassRepository StudentClassRepository { get { return _studentClassRepository; } }
        public IDocumentRepository DocumentRepository { get { return _documentRepository; } }
        public IEmailSendRepository EmailSendRepository { get { return _emailSendRepository; } }    
        public IEmailSendStudentRepository EmailSendStudentRepository { get { return _emailSendStudentRepository; } }

        public FamsDbContext DbContext { get { return _dbContext; } }





        public async Task<int> SaveChangeAsync()
        {
            return await _famsDbContext.SaveChangesAsync();

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _famsDbContext.Dispose();

                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~UnitOfWork()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
