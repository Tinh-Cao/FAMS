using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.LessonModels
{
    public class LessonDetailsModel
    {
        public int Id { get; set; }
        public string LessonCode { get; set; } = null!;

        public string? LessonName { get; set; }

        public string? Status { get; set; }

        public bool? IsDelete { get; set; } = false;
    }
}
