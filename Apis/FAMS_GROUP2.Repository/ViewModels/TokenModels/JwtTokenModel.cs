using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.TokenModels
{
    public class JwtTokenModel
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
