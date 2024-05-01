using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.DocumentModels
{
    public class CreateDocumentModel
    {
        [Required(ErrorMessage = "lessonid is required!")]
        public int LessonId { get; set; }
        public String DocumentName { get; set; }
        [Required(ErrorMessage = "DocumentLink is required!")]
        public String DocumentLink { get; set; }
    }
}
