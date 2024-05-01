using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.AttendanceModels
{
    public class AttendanceClassResultModel
    {
        public bool status { get;set; }
        public string? message { get; set; }
        public List<object?>? listObject { get; set; } = new();
        public static AttendanceClassResultModel ReturnResult(bool status,string?message, List<object?>? listObject)
        {
            return new AttendanceClassResultModel
            {
                status = status,
                message = message,
                listObject = listObject
            };
        }
    }
}
