using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.ResponseModels
{
    public class AccountImportResponseModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public List<AccountImportModel> ExistingAccounts { get; set; }
    }
}
