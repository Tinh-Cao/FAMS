using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Repositories
{
    public class AttendanceClassRepository : GenericRepository<AttendanceClass> , IAttendanceClassRepository
    {
        private readonly FamsDbContext _context;
        
        public AttendanceClassRepository(FamsDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(
       context, timeService, claimsService)
        {
            _context = context;
        }
        public async Task<List<AttendanceClass>> GetAllAttendanceClassAsync(int? studentClassId)
        {
            var list = _context.AttendanceClasses.Where(l => l.IsDelete == false);
            if (studentClassId == null) return await list.ToListAsync();
            else return await list.Where(l => l.StudentClassId == studentClassId).ToListAsync();
        }
        public async Task<List<AttendanceClass>> GetAllAttendanceClassIsDeleteAsync(int? studentClassId)
        {
            var list = _context.AttendanceClasses.Where(l => l.IsDelete == true);
            if (studentClassId == null) return await list.ToListAsync();
            else return await list.Where(l => l.StudentClassId == studentClassId).ToListAsync();
        }
        public async Task AddAttendanceClassAsync(AttendanceClass attendanceClass)
        {
            if (attendanceClass != null) await AddAsync(attendanceClass);
        }

        public async Task UpdateAttendanceClassAsync(AttendanceClass attendanceClass)
        {
            if (attendanceClass != null) await Update(attendanceClass);
        }
        
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
