using FAMS_GROUP2.Repositories.Entities;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.AccountModels
{
    public class AccountAddRangeModel
    {
        public Account? Account { get; set; } = null;
        public string rolename { get; set; }
    }
}
