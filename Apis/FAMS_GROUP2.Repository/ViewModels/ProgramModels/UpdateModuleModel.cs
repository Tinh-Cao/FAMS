using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.ProgramModels
{
    public class UpdateModuleModel
    {
       
        [Display(Name = "Module Id")]
        public List<int>? Id { get; set; }
        //[Display(Name = "Module Name")]
        //public string ModuleName { get; set; }
        //public string Status { get; set; }
    }
}
