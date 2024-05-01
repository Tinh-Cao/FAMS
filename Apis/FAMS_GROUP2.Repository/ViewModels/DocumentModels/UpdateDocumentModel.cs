using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.DocumentModels
{
    public class UpdateDocumentModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "DocumentLink is required!")]
        public string DocumentLink { get; set; } = null!;
        public string? DocumentName { get; set; }
    }
}
