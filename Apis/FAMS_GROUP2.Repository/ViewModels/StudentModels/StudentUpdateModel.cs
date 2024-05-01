using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.StudentModels
{
    public class StudentUpdateModel
    {
        [Required(ErrorMessage = "Full Name is required!")]
        [Display(Name = "FullName")]
        public string? FullName { get; set; }
   
        [Required(ErrorMessage = "Phone number is required!")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number!")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number!")]

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Date of Birth is required!")]
        [Display(Name = "Date of Birth")]
        public DateTime? Dob { get; set; }
        [Required(ErrorMessage = "Gender is required!")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "University is required!")]
        [Display(Name = "University")]
        public string? University { get; set; }
        [Display(Name = "Major")]
        public string? Major { get; set; }
        [Display(Name = "YearOfGraduation")]
        public decimal? YearOfGraduation { get; set; }
        [Display(Name = "Gpa")]
        public double? Gpa { get; set; }

        [Display(Name = "EnrollmentArea")]
        public string? EnrollmentArea { get; set; }
        [Display(Name = "Recruiter")]
        public string? Recruiter { get; set; }
        [Required(ErrorMessage = "Skill is required!")]
        [Display(Name = "Skill")]
        public string? Skill { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image")]
        public string? Image { get; set; } = null;
    }
}
