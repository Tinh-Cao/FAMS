using Application.ViewModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAMS_GROUP2.Repositories.ViewModels.AssignmentModels;

namespace FAMS_GROUP2.Repositories.ViewModels.ResponseModels
{
    public class AssignmentResponseModel : ResponseModel
    {
        public List<AssignmentImportModel> DuplicatedNameAsm { get; set; }
    }
}

