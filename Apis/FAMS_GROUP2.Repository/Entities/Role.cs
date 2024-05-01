using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Entities
{
    public class Role : BaseEntity
    {
        public string? RoleName { get; set; } = "";

        public virtual ICollection<Account> Account { get; set; } = new List<Account>();
    }
}
