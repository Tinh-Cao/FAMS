using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.ViewModels.EmailModel;
using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Services.Interfaces
{
    public interface IMailService
    {
        Task<ResponseModel> SendEmailAsync(MailRequest mailRequest);

    }
}
