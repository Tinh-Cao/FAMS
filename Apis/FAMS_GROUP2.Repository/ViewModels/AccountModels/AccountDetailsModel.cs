using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.AccountModels
{
    public class AccountDetailsModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Image {  get; set; }
        public bool isDelete { get; set; }
        public string Role { get; set; }   

    }
}
