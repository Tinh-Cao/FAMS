using System;
using System.Collections.Generic;

namespace FAMS_GROUP2.Repositories.Entities;

public partial class EmailTemplate : BaseEntity
{

    public string? Type { get; set; }

    public string? Name { get; set; }

    public string? Content { get; set; }

    public virtual ICollection<EmailSend> EmailSends { get; set; } = new List<EmailSend>();
}
