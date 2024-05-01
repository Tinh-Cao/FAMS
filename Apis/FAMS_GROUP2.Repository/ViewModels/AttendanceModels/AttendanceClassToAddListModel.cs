using FAMS_GROUP2.Repositories.Interfaces.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.AttendanceModels
{
    public class AttendanceClassToAddListModel
    {
        public int ClassId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DaysOfWeekModel? DayOfWeek { get; set; }
        public List<DateTime>? ListExclusionDate { get; set; }
    }
}
