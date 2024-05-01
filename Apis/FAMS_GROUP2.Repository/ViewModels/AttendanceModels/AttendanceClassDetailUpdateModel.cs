using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.AttendanceModels
{
    public class AttendanceClassDetailUpdateModel
    {
        public int ClassId { get; set; }
        public DateTime Date { get; set; }
        public List<AttendanceClassStudentDetail> ListStudents { get; set; } = new();

    }
}
