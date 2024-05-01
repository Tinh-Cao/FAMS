using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.ResponseModels
{
    public class EmailTemplateResponseModel
    {
        public string? Type { get; set; }

        public string? Name { get; set; }

        public string? Content { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string? CreateBy { get;set; }
    }
}
