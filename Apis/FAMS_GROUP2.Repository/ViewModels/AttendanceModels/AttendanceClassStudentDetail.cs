using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.AttendanceModels
{
    public class AttendanceClassStudentDetail
    {
        public int StudentId { get; set; }
        public string? Status { get; set; }
        public string? Comment { get; set; }
    }
}
