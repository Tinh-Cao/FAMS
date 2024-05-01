using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.CertificateModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Services.Interfaces
{
    public interface ICertificateService
    {
        Task<ResponseModel> CreateCertificateAsync(CertificateModel certificateCreateModel);
        Task<ResponseModel> UpdateCertificateAsync(int certificateId, CertificateModel certificateUpdateModel);
        Task<ResponseModel> DeleteCertificateAsync(int certificateId);
        Task<ResponseModel> UnDeleteCertificateAsync(int certificateId);
        Task<ResponseModel> ProvideCertificateAsync(CertificateProvideModel certificateModels);
        Task<CertificateViewModel> GetCertificateAsync(int studentId, int classId);
        Task<List<CertificateViewModel>> GetAllStudentCertificateAsync(int studentId);
        Task<List<CertificateModel>> GetAllCertificateTemplate();
    }
}
