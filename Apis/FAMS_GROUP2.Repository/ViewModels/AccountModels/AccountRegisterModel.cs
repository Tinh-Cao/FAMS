using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.AccountModels
{
    public class AccountRegisterModel
    {
        [Required(ErrorMessage = "Email is required!"), EmailAddress(ErrorMessage = "Must be email format!")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is required!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be 6-20 Character")]
        [PasswordPropertyText]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Confirm Password is required!")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation does not match!")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be 6-20 Character")]
        [PasswordPropertyText]
        public string ConfirmPassword { get; set; } = "";

        [Required(ErrorMessage = "Full Name is required!")]
        [Display(Name = "FullName")]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = "Phone number is required!")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number!")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number!")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = "";

        [Required(ErrorMessage = "Date of Birth is required!")]
        [Display(Name = "Date of Birth")]
        public DateTime Dob { get; set; }

        [Required(ErrorMessage = "Gender is required!")]
        [Display(Name = "Gender")]
        public bool Gender { get; set; }

        [Required(ErrorMessage = "Address is required!")]
        [Display(Name = "Address")]
        public string Address { get; set; } = "";
    }
}
