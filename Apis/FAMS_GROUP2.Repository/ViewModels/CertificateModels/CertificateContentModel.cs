using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.CertificateModels
{
    public class CertificateContentModel
    {
        public string? FullName { get; set; }
        public string GPA { get; set; }
        public string IssueDate { get; set; }
        public string? CourseName { get; set; }
    }
}
