using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.DocumentModels
{
    public class DocumentDetailsModel
    {
        public int Id { get; set; }
        public string DocumentLink { get; set; } = null!;

        public string? DocumentName { get; set; }

        public bool? IsDelete { get; set; } = false;
    }
}
