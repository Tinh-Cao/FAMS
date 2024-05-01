using System;
using System.Collections.Generic;

namespace FAMS_GROUP2.Repositories.Entities;

public partial class EmailSend : BaseEntity
{

    public int? SenderId { get; set; }

    public int? TemplateId { get; set; }

    public DateTime? SendDate { get; set; }
    public string? Subject { get; set; }

    public string? Content { get; set; }

    public virtual ICollection<EmailSendStudent> EmailSendStudents { get; set; } = new List<EmailSendStudent>();

    public virtual Account? Sender { get; set; }

    public virtual EmailTemplate? Template { get; set; }
}
