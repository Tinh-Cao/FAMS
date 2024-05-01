using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.AccountModels
{
    public class AccountUpdateModel
    {
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = "Phone number is required!")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number!")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number!")]
        public string PhoneNumber { get; set; } = "";

        [Required(ErrorMessage = "Date of Birth is required!")]
        public string Dob { get; set; } = "";

        [Required(ErrorMessage = "Gender is required!")]
        public string Gender { get; set; } = "";

        [Required(ErrorMessage = "Address is required!")]
        public string Address { get; set; } = "";
        public string Role { get; set; } = "";
    }
}
