using System;
using System.Collections.Generic;

namespace FAMS_GROUP2.Repositories.Entities;

public partial class TrainingProgram : BaseEntity
{
    public string ProgramCode { get; set; } = null!;

    public string? ProgramName { get; set; }

    public string? Duration { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<ProgramModule> ProgramModules { get; set; } = new List<ProgramModule>();
}
