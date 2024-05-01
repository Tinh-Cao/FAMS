using FAMS_GROUP2.Repositories.ViewModels.CertificateModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Interfaces.UoW
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
        IAssignmentRepository AssignmentRepository { get; }
        IClassRepository ClassRepository { get; }
        IEmailTemplateRepository EmailTemplateRepository { get; }
        IScoreRepository ScoreRepository { get; }
        IUserManager UserManager { get; }
        ILessonRepository LessonRepository { get; }
        IModuleRepository ModuleRepository { get; }
        IProgramModuleRepository ProgramModuleRepository { get; }
        IProgramRepository ProgramRepository { get; }
        IStudentRepository StudentRepository { get; }
        IAttendanceClassRepository AttendanceClassRepository { get; }
        IStudentClassRepository StudentClassRepository { get; }
        ICertificateRepository CertificateRepository { get; }
        IDocumentRepository DocumentRepository { get; } 
        IEmailSendRepository EmailSendRepository { get; }
        IEmailSendStudentRepository EmailSendStudentRepository { get; }
        IClassAccountRepository ClassAccountRepository { get; }
        FamsDbContext DbContext { get; }

        public Task<int> SaveChangeAsync();
    }
}
