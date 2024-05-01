using FAMS_GROUP2.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.ModuleModels
{
    public class CreateModuleViewModel
    {
        [Required(ErrorMessage = "ModuleCode is required!")]
        public string ModuleCode { get; set; } = null!;

        public string? ModuleName { get; set; }
        [RegularExpression("^(Active|Stop|Pending)$", ErrorMessage = "Status must be Active or Stop or Pending!")]
        public string Status { get; set; } = ModuleStatus.Active.ToString();
        


    }
}
