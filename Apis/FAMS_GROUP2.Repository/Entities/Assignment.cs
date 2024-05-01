using System;
using System.Collections.Generic;

namespace FAMS_GROUP2.Repositories.Entities;

public partial class Assignment : BaseEntity
{

    public int? ModuleId { get; set; }

    public string? AssignmentName { get; set; }
    
    public string? Content { get; set; }

    public string? Status { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? AssignmentType { get; set; }

    public virtual Module? Module { get; set; }
}
