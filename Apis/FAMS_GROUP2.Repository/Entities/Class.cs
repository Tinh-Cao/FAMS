using System;
using System.Collections.Generic;

namespace FAMS_GROUP2.Repositories.Entities;

public partial class Class : BaseEntity
{

    public string? ClassName { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Location { get; set; }

    public int? ProgramId { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<ClassAccount> ClassAccounts { get; set; } = new List<ClassAccount>();

    public virtual TrainingProgram? Program { get; set; }

    public virtual ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();

    public virtual ICollection<Score> Scores { get; set; } = new List<Score>();
}
