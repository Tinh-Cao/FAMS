namespace FAMS_GROUP2.Repositories;

public class ClassItemModel
{
    public int Id { get; set; }
    public string? ClassName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Location { get; set; } = string.Empty;
    public string? ProgramName { get; set; } // for better performance when show class list
    public int? ProgramId { get; set; }
    public string? ProgramCode { get; set; }
    public int? AdminId { get; set; }
    public int? TrainerId { get; set; }
    public string? Status { get; set; }
}