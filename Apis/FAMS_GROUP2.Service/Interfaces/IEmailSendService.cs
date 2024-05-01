using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.EmailSendsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Services.Interfaces
{
    public interface IEmailSendService
    {
        Task<EmailSend> GetSendMailByIdAsync(int id);
        Task<Pagination<EmailSend>> GetAllEmailSendFilterBySendDateAsync(PaginationParameter paginationParameter, EmailSendsFilterModule emailSendsFilterModule);
    }
}
