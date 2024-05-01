using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.EmailModel
{
    public class EmailTemplateModel
    {
        [Required(ErrorMessage = "Type is required!")]
        [Display(Name = "Type")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required!")]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required!")]
        [Display(Name = "Content")]
        public string Content { get; set; } = string.Empty;
    }
}
