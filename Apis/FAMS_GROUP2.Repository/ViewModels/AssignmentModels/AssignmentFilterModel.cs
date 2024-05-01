using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.AssignmentModels
{
    public class AssignmentFilterModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Sort { get; set; } = "id";
        public string SortDirection { get; set; } = "desc";
        public string? Search { get; set; }
        public string? Status { get; set; }
        public int? ModuleId { get; set; }
        public string? Type { get; set; }
    }
}
