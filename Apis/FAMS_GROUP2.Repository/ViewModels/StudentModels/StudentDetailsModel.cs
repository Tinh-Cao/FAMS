using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.StudentModels
{
    public class StudentDetailsModel
    {
        public int Id { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? Dob { get; set; }

        public String? Gender { get; set; }

        public string? University { get; set; }

        public string? Major { get; set; }

        public decimal? YearOfGraduation { get; set; }

        public double? Gpa { get; set; }


        public string? StudentCode { get; set; }

        public string? EnrollmentArea { get; set; }

        public string? Recruiter { get; set; }

        public string? Skill { get; set; }
        public string? Status { get; set; }
        public string? Image { get; set; }
        public bool? IsDelete { get; set; } 

    }
}