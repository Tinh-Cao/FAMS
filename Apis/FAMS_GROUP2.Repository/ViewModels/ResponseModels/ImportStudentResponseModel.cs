using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.ViewModels.StudentModels;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FAMS_GROUP2.Repositories.ViewModels.ResponseModels
{
    public class ImportStudentResponseModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<StudentImportModel>? DuplicatedStudents { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<StudentImportModel>? AddedStudents { get; set; } 

        public string? Message { get; set; }
        public bool? Status { get; set; } = true;
    }
}