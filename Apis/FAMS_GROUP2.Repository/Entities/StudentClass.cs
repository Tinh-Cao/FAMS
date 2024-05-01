using System;
using System.Collections.Generic;

namespace FAMS_GROUP2.Repositories.Entities;

public partial class StudentClass : BaseEntity
{
    public int? StudentId { get; set; }

    public int? ClassId { get; set; }

    public virtual ICollection<AttendanceClass> AttendanceClasses { get; set; } = new List<AttendanceClass>();

    public virtual Class? Class { get; set; }

    public virtual Student? Student { get; set; }
}
