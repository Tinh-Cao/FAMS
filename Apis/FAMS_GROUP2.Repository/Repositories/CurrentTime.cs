using FAMS_GROUP2.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Repositories
{
    public class CurrentTime : ICurrentTime
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.UtcNow.AddHours(7);
        }
    }
}
