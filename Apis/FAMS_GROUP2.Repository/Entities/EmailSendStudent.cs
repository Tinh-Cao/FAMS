using System;
using System.Collections.Generic;

namespace FAMS_GROUP2.Repositories.Entities;

public partial class EmailSendStudent : BaseEntity
{

    public int? ReceiveId { get; set; }

    public int? EmailSendId { get; set; }

    public virtual EmailSend? EmailSend { get; set; }

    public virtual Student? Receive { get; set; }
}
