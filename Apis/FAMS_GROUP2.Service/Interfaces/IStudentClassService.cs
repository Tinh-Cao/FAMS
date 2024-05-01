using FAMS_GROUP2.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Services.Interfaces
{
    public interface IStudentClassService
    {
        public Task<IEnumerable<StudentClass>> GetStudentClass();
        public Task<IEnumerable<StudentClass>> GetStudentClassIsDelete();


    }
}
