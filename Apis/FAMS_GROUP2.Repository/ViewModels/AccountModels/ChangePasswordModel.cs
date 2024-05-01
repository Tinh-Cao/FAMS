using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.AccountModels
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Must be email formated!")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Current Password is required!")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be 6-20 Character")]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New Password is required!")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be 6-20 Character")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm new Password is required!")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be 6-20 Character")]
        [Display(Name = "Confirm new Password")]
        [Compare("NewPassword", ErrorMessage = "New password and confirmation does not match!")]
        public string ConfirmNewPassword { get; set; }
    }
}
