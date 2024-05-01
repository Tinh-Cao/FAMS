using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.ViewModels.EmailModel;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Services.Interfaces
{
    public interface IEmailTemplateServices
    {
        Task<EmailTemplate> AddEmailTemplateAsync(EmailTemplateModel emailTemplate);
        Task<List<EmailTemplate>> GetAllEmailTemplateAsync();
        Task<EmailTemplate> GetAllEmailTemplateByIdAsync(int id);
        Task<EmailTemplate> UpdateEmailTemplateAsync(int id, EmailTemplateModel model);
        Task<EmailTemplate> BanEmailTemplateAsync(int id);  
        Task<EmailTemplate> UnBanEmailTemplateAsync(int id);
        Task<ResponseModel> DeleteEmailTemplateAsync(int id);
    }
}
