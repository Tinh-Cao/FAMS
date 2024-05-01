using System.ComponentModel.DataAnnotations;

namespace FAMS_GROUP2.Repositories;

public class UpdateClassModel
{
    [Required(ErrorMessage = "ClassName is required!")]
    [Display(Name = "Class Name")]
    public string ClassName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start date is required!")]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End date is required!")]
    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Location is required!")]
    [Display(Name = "Location")]
    public string Location { get; set; } = string.Empty;

    [Display(Name = "ProgramId")] public int? ProgramId { get; set; }

    [Required(ErrorMessage = "Status is required!")]
    [Display(Name = "Status")]
    public string Status { get; set; }

    public int? AdminId { get; set; }
    public int? TrainerId { get; set; }
}