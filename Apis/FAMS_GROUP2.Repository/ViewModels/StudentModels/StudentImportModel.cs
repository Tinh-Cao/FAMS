using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.StudentModels
{
    public class StudentImportModel
    {
        [Required(ErrorMessage = "Full Name is required!")]
        [Display(Name = "FullName")]
        public string FullName { get; set; } = "";
        [Required(ErrorMessage = "Email is required!"), EmailAddress(ErrorMessage = "Must be email format!")]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }
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
        public String Gender { get; set; } = "";
        // [Required(ErrorMessage = "University is required!")]
        [Display(Name = "University")]
        public string? University { get; set; }
       // [Required(ErrorMessage = "Major is required!")]
        [Display(Name = "Major")]
        public string? Major { get; set; }
      //  [Required(ErrorMessage = "Graduation year is required!")]
        [Display(Name = "YearOfGraduation")]
        public decimal? YearOfGraduation { get; set; }
     //   [Required(ErrorMessage = "Gpa year is required!")]
        [Display(Name = "Gpa")]
        public double? Gpa { get; set; }
        [Required(ErrorMessage = "Student code is required!")]
        [Display(Name = "StudentCode")]
        [RegularExpression(@"^SA\d{5}$", ErrorMessage = "Student code must be in the format 'SAXXXXX' where XXXXX is 5 digits.")]

        public string? StudentCode { get; set; }
      //  [Required(ErrorMessage = "Area  is required!")]
        [Display(Name = "EnrollmentArea")]
        public string? EnrollmentArea { get; set; }
       // [Required(ErrorMessage = "Recruiter is required!")]
        [Display(Name = "Recruiter")]
        public string? Recruiter { get; set; }
        [Required(ErrorMessage = "Skill is required!")]
        [Display(Name = "Skill")]
        public string? Skill { get; set; }

        //   [Required(ErrorMessage = "Image is required!")]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image")]
        public string? Image { get; set; } = null;


    }
}
