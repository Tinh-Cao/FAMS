using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.ScoreModels
{
    public class ScoreFilterModel
    {
        public int? ClassId { get; set; }

        public string Sort { get; set; } = "id";

        public string SortDirection { get; set; } = "desc";

        public bool IsDelete { get; set; }

        public string? Status { get; set; }

        public int? LevelModule { get; set; }

        public string? Search { get; set; }
    }
}
