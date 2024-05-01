using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.LessonModels
{
    public class CreateLessonModel
    {
        [Required(ErrorMessage = "ModuleId is required!")]
        public int moduleId { get; set; }
        public String LessonName { get; set; }
        [Required(ErrorMessage = "LessonCode is required!")]
        public String LessonCode { get; set; }
        [RegularExpression("^(Ongoing|Finished)$", ErrorMessage = "Status must be Ongoing or Finished!")]
        public String Status { get; set; }
    }
}
