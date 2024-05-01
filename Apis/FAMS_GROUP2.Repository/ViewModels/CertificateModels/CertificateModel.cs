using FAMS_GROUP2.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.CertificateModels
{
    public class CertificateModel
    {
        public string CertificateName { get; set; }
        public string? CertificateType { get; set; }
        public string Content { get; set; }
    }
}
