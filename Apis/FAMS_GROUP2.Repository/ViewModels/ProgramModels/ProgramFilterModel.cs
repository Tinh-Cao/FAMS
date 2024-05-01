using FAMS_GROUP2.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.ProgramModels
{
   public class ProgramFilterModel
    {
        public string Sort { get; set; } = "id";
        public string SortDirection { get; set; } = "desc";
        public string? Search { get; set; }
        public string? Duration { get; set; }
        [RegularExpression("^(Active|Pending|Stop)$", ErrorMessage = "Status must be Active, Pending or Stop!")]
        public string? Status { get; set; } //= ProgramStatus.Active.ToString();

        public bool? isDelete { get; set; }
    }
}
    