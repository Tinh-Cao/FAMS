using FAMS_GROUP2.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Interfaces
{
    public interface IAttendanceClassRepository :IGenericRepository<AttendanceClass>
    {
        public Task<List<AttendanceClass>> GetAllAttendanceClassAsync(int? studentClassId);
        public Task<List<AttendanceClass>> GetAllAttendanceClassIsDeleteAsync(int? studentClassId);
        public Task AddAttendanceClassAsync(AttendanceClass attendanceClass);
        public Task UpdateAttendanceClassAsync(AttendanceClass attendanceClass);
        public Task SaveChangesAsync();
    }
}
