using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.ViewModels.CertificateModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Interfaces
{
    public interface ICertificateRepository : IGenericRepository<Certificate>
    {
        Task<Certificate> GetCertificateByTypeAsync(string certificateType);
        Task<StudentCertificate> GetCertificateByStudentIdAsync(int studentId);
        Task<List<StudentCertificate>> GetAllCertificatesByStudentIdAsync(int studentId);
        Task<bool> AddCertificateToStudentAsync(int studentId, int certificateId, string studentCertificateContent);
        Task<bool> CheckCertificateByStudentAndClassAsync(int studentId, int classId);
    }
}
