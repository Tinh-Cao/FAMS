using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.AttendanceModels
{
    public class AttendanceClassDetailModel
    {
        public int? studentId { get; set; }
        public string? studentCode { get; set; }
        public string? fullName { get; set; }
        public string? status { get; set; }
        public string? comment { get; set; }
    }
}
