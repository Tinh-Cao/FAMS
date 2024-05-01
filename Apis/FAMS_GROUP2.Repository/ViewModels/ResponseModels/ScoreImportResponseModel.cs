using FAMS_GROUP2.Repositories.ViewModels.ScoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.ResponseModels
{
    public class ScoreImportResponseModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public List<ScoreImportModel> ExistingScores { get; set; }
    }
}
