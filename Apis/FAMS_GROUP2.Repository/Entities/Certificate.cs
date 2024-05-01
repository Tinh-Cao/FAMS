using System;
using System.Collections.Generic;

namespace FAMS_GROUP2.Repositories.Entities;

public partial class Certificate : BaseEntity
{


    public string? CertificateName { get; set; }

    public string? CertificateType { get; set; }
    public string? Content { get; set; }

    public virtual ICollection<StudentCertificate> StudentCertificates { get; set; } = new List<StudentCertificate>();
}
