using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Utils;
using System.Security.Claims;

namespace FAMS_GROUP2.API.Services
{
    public class ClaimsService : IClaimsService
    {
        public ClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            // todo implementation to get the current userId
            var identity = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var extractedId = AuthenTools.GetCurrentAccountId(identity);
            GetCurrentUserId = string.IsNullOrEmpty(extractedId) ? 0 : int.Parse(extractedId);
        }

        public int GetCurrentUserId { get; }
    }
}
