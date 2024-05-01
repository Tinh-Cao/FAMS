using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Interfaces
{
    public interface IStudentClassRepository : IGenericRepository<StudentClass>
    {
        public Task<int> GetNumberOfStudent(int classId);
        Task<List<Student>> getStudentFromAClass(int classId);
    }
}
