using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAMS_GROUP2.Repositories.Enums;

namespace FAMS_GROUP2.Repositories.ViewModels.AssignmentModels
{
    public class AssignmentImportModel
    {
            [Required(ErrorMessage = "AssignmentName is required!")]
            [Display(Name = "Assignment Name")]
            public string? AssignmentName { get; set; }

            [Required(ErrorMessage = "Content is required!")]
            public string? Content { get; set; }
            
            [EnumDataType(typeof(AssignmentStatus), ErrorMessage = "Check Status again!")]
            public string? Status { get; set; }

            [Required(ErrorMessage = "Start date is required!")]
            [Display(Name = "Start Date")]
            public DateTime? StartDate { get; set; }

            [Required(ErrorMessage = "End date is required!")]
            [Display(Name = "End Date")]
            [DateRangeValidateCustom("StartDate")]
            public DateTime? EndDate { get; set; }

            [Required(ErrorMessage = "AssignmentType is required!")]
            [Display(Name = "Assignment Type")]
            public string? AssignmentType { get; set; }
    }
}
