using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Helper
{
    public static  class EnumHelper
    {
        public static RoleEnums ConvertToRoleEnum(string role)
        {
            Console.WriteLine(role);
            switch (role.ToLower())
            {
                case "superadmin":
                    return RoleEnums.SuperAdmin;
                case "admin":
                    return RoleEnums.Admin;
                case "trainer":
                    return RoleEnums.Trainer;
                case "student":
                    return RoleEnums.Student;
                default:
                    throw new ArgumentException("Invalid role value.");
            }
        }
        public static int ConvertToRoleId(string role)
        {
            switch (role.ToLower())
            {
                case "superadmin":
                    return 1;
                case "admin":
                    return 2;
                case "trainer":
                    return 3;
                default:
                    throw new ArgumentException("Invalid role value.");
            }
        }
    }
}
