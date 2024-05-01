using FAMS_GROUP2.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.ProgramModels
{
    public class UpdateProgramModel
    {

        //[Required(ErrorMessage = "Training Program Name is required!")]
        [Display(Name = "Training Program Name")]
        public string? ProgramName { get; set; }


        //[Required(ErrorMessage = "Duration is required!")]
        [Display(Name = "Duration")]
        public string? Duration { get; set; }

       // [Required(ErrorMessage = "Status is required!")]
        [Display(Name = "Status")]
        [RegularExpression("^(Active|Pending|Stop)$", ErrorMessage = "Status must be Abctive, Pending or Stop!")]
        public string? Status { get; set; }
        
        public List<int>? ListIdForAdd{ get; set; }
        public List<int>? ListIdForRemove { get; set; }
    }
}
