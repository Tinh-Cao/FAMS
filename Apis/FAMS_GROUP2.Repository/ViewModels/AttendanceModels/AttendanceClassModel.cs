using FAMS_GROUP2.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.AttendanceModels
{
    public class AttendanceClassModel
    {
        public int Id { get; set; }
        public int? StudentClassId { get; set; }

        public DateTime? Date { get; set; }

        public string? Status { get; set; }

        public string? Comment { get; set; }

        public static AttendanceClassModel ObjectModelConvert(AttendanceClass obj)
        {
            return new AttendanceClassModel
            {
                Id = obj.Id,
                StudentClassId = obj.StudentClassId,
                Date = obj.Date,
                Status = obj.Status,
                Comment = obj.Comment
            };
        }
        public static List<AttendanceClassModel> ListModelConvert(List<AttendanceClass> list)
        {
            List<AttendanceClassModel> listModel = new();
            foreach (var item in list)
            {
                listModel.Add(ObjectModelConvert(item));
            }
            return listModel;
        }
    }
}
