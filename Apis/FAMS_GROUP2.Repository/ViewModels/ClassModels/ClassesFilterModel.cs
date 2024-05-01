using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;

namespace FAMS_GROUP2.Repositories;

public class ClassesFilterModel
{
    public string Sort { get; set; } = "id"; 
    public string SortDirection { get; set; } = "desc";
    public string? Search { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? TrainingProgramId { get; set; }
    public string? Status { get; set; }
}