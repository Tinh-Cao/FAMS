using FAMS_GROUP2.Repositories.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.AccountModels
{
    public class AccountImportModel
    {

        [JsonProperty("FullName")]
        public string FullName { get; set; } = "";

        [JsonProperty("Email")]
        [Required(ErrorMessage = "Email is required!"), EmailAddress(ErrorMessage = "Must be email format!")]
        public string Email { get; set; } = "";

        [JsonProperty("PhoneNumber")]
        [Required(ErrorMessage = "Phone number is required!")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number!")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number!")]
        public string PhoneNumber { get; set; } = "";

        [JsonProperty("Dob")]
        [Required(ErrorMessage = "Date of Birth is required!")]
        public string Dob { get; set; }

        [JsonProperty("Gender")]
        [Required(ErrorMessage = "Gender is required!")]
        public string Gender { get; set; } = "";

        [JsonProperty("Address")]
        [Required(ErrorMessage = "Address is required!")]
        public string Address { get; set; } = "";

        [JsonProperty("Role")]
        public string Role { get; set; }

    }
}

