using FAMS_GROUP2.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.ModuleModels
{
    public class ModuleFilterModule
    {
        public string SortDirection { get; set; } = "asc";
        public string Sort { get; set; } = "id";
        public bool isDelete { get; set; }  
        public string? Status { get; set; } 
        public string? Search { get; set; }

    }
}
