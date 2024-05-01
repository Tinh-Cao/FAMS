
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.ViewModels.AttendanceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Services.Interfaces
{
    public interface IAttendanceClassService
    {
        public Task<List<AttendanceClass>> GetAttendanceClass(int? studentClassId);
        public Task<List<AttendanceClass>> GetAttendanceClassIsDelete(int? studentClassId);
        public Task<List<AttendanceClassModel>> GetAttendanceClassDTO(int? studentClassId);
        public Task<AttendanceClassResultModel> GetAttendanceClassByClass(int classID, DateTime fromDate, DateTime toDate);
        public Task<AttendanceClassResultModel> GetAttendanceClassByDate(int classId, DateTime date);
        public Task<AttendanceClassResultModel> GetAttendanceClassByStudentClass(int classId, int studentId, DateTime fromDate, DateTime toDate);
        public Task<AttendanceClassResultModel> AddAttendanceOfClass(List<AttendanceClassViewOfClassModel> request, List<AttendanceClass>? listAttendanceTransmission, bool isSaveChanges);
        public Task<AttendanceClassResultModel> AddListAttendanceOfClassByDate(AttendanceClassToAddListModel request);
        public Task AddListAttendanceOfNewStudentIntoClass(int classId, int studentId);
        public Task<AttendanceClassResultModel> UpdateAttendanceClass(AttendanceClassDateUpdateModel request);
        public Task<AttendanceClassResultModel> UpdateAttendanceClassDetail(AttendanceClassDetailUpdateModel request);
        public Task<AttendanceClassResultModel> DeleteAttendanceOfClass(AttendanceClassViewOfClassModel request);
        public Task DeleteListAttendanceOfStudentOutClass(int classId, int? studentId);
        public Task<AttendanceClassResultModel> ViewPresentAndAbsentOfClassInDate(int classId, DateTime date);
        public Task<AttendanceClassResultModel> ViewPresentAndAbsentOfStudentInClass(int classId, int studentId);
        public Task InitialStudentClassOfClass(int classId);
        public Task<bool> InitialStudentClassOfStudent(int classId, int studentId);
        public Task<bool> DeleteStudentClassOfStudent(int classId, int studentId);
        public Task DeleteAllStudentOfClass(int classId);
    }
}
