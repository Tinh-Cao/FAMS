using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.AssignmentModels
{
    public class AssignmentViewModel
    {

        public int Id { get; set; }

        public int? ModuleId { get; set; }

        public string? AssignmentName { get; set; }

        public string? Content { get; set; }
        
        public string? Status { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? AssignmentType { get; set; }

        public bool IsDeleted { get; set; }
    }
}
