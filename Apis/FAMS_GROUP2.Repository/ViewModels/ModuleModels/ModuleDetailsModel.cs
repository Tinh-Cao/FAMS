using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.ModuleModels
{
    public class ModuleDetailsModel
    {
        public int Id { get; set; }
        public string ModuleCode { get; set; } = null!;

        public string? ModuleName { get; set; }

        public string? Status { get; set; }

        public bool? IsDelete { get; set; }
      
    }
}
