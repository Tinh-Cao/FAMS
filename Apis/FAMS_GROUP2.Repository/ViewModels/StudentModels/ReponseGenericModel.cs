using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FAMS_GROUP2.Repositories.ViewModels.StudentModels
{
    public class ResponseGenericModel<TEntity> 
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; } = "";
        public TEntity Data { get; set; }

        public ResponseGenericModel(TEntity data, bool status = false, string message = "")
        {
            Data = data;
            Status = status;
            Message = message;
        }
    }
}
