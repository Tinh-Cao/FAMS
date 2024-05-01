using System;
using System.Collections.Generic;

namespace FAMS_GROUP2.Repositories.Entities;

public partial class AttendanceClass : BaseEntity
{

    public int? StudentClassId { get; set; }

    public DateTime? Date { get; set; }

    public string? Status { get; set; }

    public string? Comment { get; set; }

    public virtual StudentClass? StudentClass { get; set; }
}
