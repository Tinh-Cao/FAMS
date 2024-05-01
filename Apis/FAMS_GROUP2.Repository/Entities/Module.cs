using System;
using System.Collections.Generic;

namespace FAMS_GROUP2.Repositories.Entities;

public partial class Module : BaseEntity
{
    public string ModuleCode { get; set; } = null!;

    public string? ModuleName { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual ICollection<ProgramModule> ProgramModules { get; set; } = new List<ProgramModule>();
}
