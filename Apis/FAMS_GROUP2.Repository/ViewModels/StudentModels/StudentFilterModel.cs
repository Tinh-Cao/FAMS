using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.StudentModels
{
    public class StudentFilterModel
    {
        public string Sort { get; set; } = "id";
        public string SortDirection { get; set; } = "desc";
        public bool? isDelete { get; set; } = null;
        public string? Gender { get; set; }
        public string? Search { get; set; }
        public string? Skill { get; set; }
        public string? Status { get; set; }
        public int? ClassId { get; set; }

    }
}
