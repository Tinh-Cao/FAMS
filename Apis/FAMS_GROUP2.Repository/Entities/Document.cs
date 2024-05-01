using System;
using System.Collections.Generic;

namespace FAMS_GROUP2.Repositories.Entities;

public partial class Document : BaseEntity
{

    public int LessonId { get; set; }

    public string? DocumentName { get; set; }

    public string? DocumentLink { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;
}
