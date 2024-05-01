using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.ResponseModels
{
    public class ProgramResponseModel
    {   public string Id { get; set; }
        public string ProgramCode { get; set; }

        public string? ProgramName { get; set; }

        public string? Duration { get; set; }

        public string? Status { get; set; }
        public bool? isDelete { get; set; }

        public List<int> ModulesId { get; set; } = new List<int>();
    }
}
