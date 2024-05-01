using System;
using System.Collections.Generic;

namespace FAMS_GROUP2.Repositories.Entities;

public partial class ClassAccount : BaseEntity
{
    public int? ClassId { get; set; }

    public int? AdminId { get; set; }

    public int? TrainerId { get; set; }

    public string? Status { get; set; }

    public virtual Account? Admin { get; set; }

    public virtual Class? Class { get; set; }

    public virtual Account? Trainer { get; set; }
}
