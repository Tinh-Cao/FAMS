using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.ViewModels.EmailModel;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Interfaces
{
    public interface IEmailTemplateRepository : IGenericRepository<EmailTemplate>
    {

         Task DeleteAsync(int id);
        Task<EmailTemplate> UpdateEmailTemplate(int id, EmailTemplateModel model);

        Task<EmailTemplate> BanEmailTemplate(int id);

        Task<EmailTemplate> UnBanEmailTemplate(int id);
    }
}
