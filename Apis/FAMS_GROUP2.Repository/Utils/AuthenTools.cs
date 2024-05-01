using FAMS_GROUP2.Repositories.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Utils
{
    public class AuthenTools
    {
        private readonly FamsDbContext famsDbContext;

        public AuthenTools(FamsDbContext famsDbContext)
        {
            this.famsDbContext = famsDbContext;
        }
        public static string GetCurrentAccountId(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return userClaims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
            }
            return null;
        }

        public async Task<Account> Launch(ClaimsIdentity identity)
        {
            var extractedEmail = GetCurrentID(identity);

            if (extractedEmail == null)
            {
                throw new Exception("thua ori");
            }

            var result = GetCurrentUser(identity);

            return result;
        }

        public  Account GetCurrentUser(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                var userClaims = identity.Claims;
                var id = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var result = famsDbContext.Accounts.FirstOrDefault(x => x.Id.ToString() == id);
                return result;
            }
            return null;
        }
        //Lấy ra Email từ token
        private  string GetCurrentID(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                var userClaims = identity.Claims;
                Console.Write(userClaims.Count());

                foreach (var claim in userClaims)
                {
                    Console.WriteLine(claim.ToString());
                }

                return userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            }
            return null;
        }
    }
}
