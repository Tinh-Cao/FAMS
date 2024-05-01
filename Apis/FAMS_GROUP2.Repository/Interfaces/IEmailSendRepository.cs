using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.EmailSendsModels;
using FAMS_GROUP2.Repositories.ViewModels.ModuleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Interfaces
{
    public interface IEmailSendRepository : IGenericRepository<EmailSend>
    {
        Task<List<EmailSend>> GetAllEmailSendFilterBySendDate(PaginationParameter paginationParameter, EmailSendsFilterModule emailSendsFilterModule);
        Task<List<EmailSend>> CheckIdTemplate(int id);

        Task<List<Student>> CheckId(List<int> id);
    }
}
