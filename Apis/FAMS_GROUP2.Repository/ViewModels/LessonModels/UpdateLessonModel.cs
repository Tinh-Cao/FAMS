using FAMS_GROUP2.Repositories.ViewModels.ProgramModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.LessonModels
{
    public class UpdateLessonModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "LessonCode is required!")]
        public string LessonCode { get; set; } = null!;
        public string? LessonName { get; set; }
        public string? Status { get; set; }
    }
}
