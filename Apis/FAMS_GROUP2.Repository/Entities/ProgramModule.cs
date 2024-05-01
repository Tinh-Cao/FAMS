using System;
using System.Collections.Generic;

namespace FAMS_GROUP2.Repositories.Entities;

public partial class ProgramModule : BaseEntity
{

    public int? ProgramId { get; set; }

    public int? ModuleId { get; set; }

    public virtual Module? Module { get; set; }

    public virtual TrainingProgram? Program { get; set; }
}
