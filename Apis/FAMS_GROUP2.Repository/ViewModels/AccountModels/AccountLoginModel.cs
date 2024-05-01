using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.AccountModels
{
    public class AccountLoginModel
    {
        [Required(ErrorMessage = "Email is required!"), EmailAddress(ErrorMessage = "Must be email format!")]
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
