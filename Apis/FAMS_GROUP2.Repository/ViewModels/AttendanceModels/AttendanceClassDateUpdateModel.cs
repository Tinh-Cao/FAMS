using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.AttendanceModels
{
    public class AttendanceClassDateUpdateModel
    {
        public int ClassId { get;set; }
        public DateTime PreviousDate { get; set; }
        public DateTime AfterDate { get; set; }
        //int classId, DateTime previousDate, DateTime afterDate
    }
}
