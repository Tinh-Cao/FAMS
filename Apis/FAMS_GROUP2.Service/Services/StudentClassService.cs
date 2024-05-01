using AutoMapper;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Services.Services
{
    public class StudentClassService:IStudentClassService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public StudentClassService( IMapper mapper,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<StudentClass>> GetStudentClass()
        {
            return (await _unitOfWork.StudentClassRepository.GetAllAsync()).FindAll(l=>l.IsDelete == false);
        }
        public async Task<IEnumerable<StudentClass>> GetStudentClassIsDelete()
        {
            return (await _unitOfWork.StudentClassRepository.GetAllAsync()).FindAll(l => l.IsDelete == true);
        }


    }
}
