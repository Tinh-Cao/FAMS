using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.AccountModels
{
    public class AccountFilterModel
    {
        public string Sort { get; set; } = "id"; 
        public string SortDirection { get; set; } = "desc";
        public string? Role { get; set; }
        public bool? isDelete { get; set; }
        public string? Gender { get; set; }
        public string? Search { get; set; }
    }
}
