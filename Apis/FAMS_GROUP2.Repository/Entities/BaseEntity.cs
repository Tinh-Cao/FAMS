using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public string? DeletedBy { get; set; }

        public bool? IsDelete { get; set; } = false;
    }
}
