using System.ComponentModel.DataAnnotations;
using FAMS_GROUP2.Repositories.Enums;

namespace FAMS_GROUP2.Repositories;

public class CreateClassModel
{
    [Required(ErrorMessage = "ClassName is required!")]
    [Display(Name = "Class Name")]
    public string ClassName {get; set;} = string.Empty;

    [Required(ErrorMessage = "Start date is required!")]
    [Display(Name = "Start Date")]
    public DateTime StartDate {get; set;}

    [Required(ErrorMessage = "End date is required!")]
    [Display(Name = "End Date")]
    public DateTime EndDate {get; set;}

    [Required(ErrorMessage = "Location is required!")]
    [Display(Name = "Location")]
    public string Location {get; set;} = string.Empty;
    
    [Display(Name = "ProgramId")]
    public int? ProgramId {get; set;}
    public int? AdminId { get; set; }
}
