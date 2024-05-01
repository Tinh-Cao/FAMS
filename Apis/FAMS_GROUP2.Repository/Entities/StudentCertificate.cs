using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Entities
{
    public class StudentCertificate: BaseEntity
    {
        public int? StudentId { get; set; }

        public int CertificateId { get; set; }
        public DateTime ProvidedDate { get; set; }
        public string? CertificateCode { get; set; }
        public string? Content { get; set; }

        public virtual Certificate? Certificate { get; set; }

        public virtual Student? Student { get; set; }
    }
}
