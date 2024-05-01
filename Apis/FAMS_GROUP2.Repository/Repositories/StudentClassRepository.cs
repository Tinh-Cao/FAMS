using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FAMS_GROUP2.Repositories.Repositories
{
    public class StudentClassRepository:GenericRepository<StudentClass>,IStudentClassRepository
    {
        private readonly FamsDbContext _dbContext;

        public StudentClassRepository(FamsDbContext context, ICurrentTime timeService, IClaimsService claimsService) :
            base(
                context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Student>> getStudentFromAClass(int classId)
        {
            var students = new List<Student>();
            var targetClass = await _dbContext.Classes.FirstOrDefaultAsync(x => x.Id == classId);
            if (targetClass == null)
            {
                return students;
            }
            students = await _dbContext.StudentClasses.Where(
                sc => sc.ClassId == targetClass.Id && sc.StudentId != null && sc.IsDelete == false).Select(sc => sc.Student).ToListAsync();

            if( students.Count == 0 )
            {
                return students ;
            }


            return students;
            
        }


        public async Task<Student> GetStudentAsync(int accountId)
        {
            var account = await _dbContext.Students.FindAsync(accountId);
            return account;
        }


        public async Task<int> GetNumberOfStudent(int classId)
        {
            var studentClassInfo = await _dbContext.StudentClasses.ToListAsync();
            return studentClassInfo.Count(a => a.ClassId == classId && a.StudentId != null);
        }
    }
}
