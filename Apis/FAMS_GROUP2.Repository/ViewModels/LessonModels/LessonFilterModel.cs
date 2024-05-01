using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.LessonModels
{
    public class LessonFilterModel
    {
        public string Sort { get; set; } = "id";
        public string SortDirection { get; set; } = "desc";
        public bool isDelete { get; set; } 
        public string? Status { get; set; }
        public string? Search { get; set; }
        public int? ModuleId { get; set; }
    }
}
