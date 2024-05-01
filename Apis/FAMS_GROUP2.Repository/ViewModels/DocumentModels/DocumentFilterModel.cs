using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.DocumentModels
{
    public class DocumentFilterModel
    {
        public string Sort { get; set; } = "id";
        public string SortDirection { get; set; } = "desc";
        public bool isDelete { get; set; }
        public string? Search { get; set; }
        public int? LessonId { get; set; }
    }
}
