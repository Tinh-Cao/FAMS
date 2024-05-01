using System;
using System.Collections.Generic;

namespace FAMS_GROUP2.Repositories.Entities;

public partial class Lesson : BaseEntity
{
    public int? ModuleId { get; set; }

    public string? LessonName { get; set; }

    public string? LessonCode { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual Module? Module { get; set; }
}
