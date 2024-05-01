using FAMS_GROUP2.Repositories.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.ProgramModels
{
    public class ProgramModel
    {
        [Required(ErrorMessage = "Program Code is required!")]
        [Display(Name = "Training Program Code")]
        public string ProgramCode { get; set; } = null!;

        [Required(ErrorMessage = "Training Program Name is required!")]
        [Display(Name = "Training Program Name")]
        public string ProgramName { get; set; }


        [Required(ErrorMessage = "Duration is required!")]
        [Display(Name = "Duration")]
        public string Duration { get; set; }

        //[Required(ErrorMessage = "Status is required!")]
        //[Display(Name = "Status")]
        //[RegularExpression("^(Active|Stop|Pending)$", ErrorMessage = "Status must be Active, Pending or Stop!")]
        //public string Status { get; set; }

    }
}
