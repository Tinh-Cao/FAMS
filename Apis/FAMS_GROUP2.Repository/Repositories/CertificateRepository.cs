using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.ViewModels.CertificateModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Repositories
{
    public class CertificateRepository : GenericRepository<Certificate>, ICertificateRepository
    {
        private readonly FamsDbContext _dbContext;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        public CertificateRepository(FamsDbContext context,
            ICurrentTime currentTime, IClaimsService claimsService) :
            base(context, currentTime, claimsService)
        {
            _dbContext = context;
            _currentTime = currentTime;
            _claimsService = claimsService;
        }

        public async Task<Certificate> GetCertificateByTypeAsync(string certificateType)
        {
            var certificate = await _dbContext.Certificates.FirstOrDefaultAsync(c => c.CertificateType == certificateType);
            return certificate;
        }
        public async Task<StudentCertificate> GetCertificateByStudentIdAsync(int studentId)
        {
            var studentScertificate = await _dbContext.StudentCertificates.FirstOrDefaultAsync(sc => sc.StudentId == studentId);
            return studentScertificate;
        }

        public async Task<List<StudentCertificate>> GetAllCertificatesByStudentIdAsync(int studentId)
        {
            var studentCertificates = await _dbContext.StudentCertificates
                .Where(sc => sc.StudentId == studentId)
                .ToListAsync();
            return studentCertificates;
        }



        //public async Task<bool> GetCertificateByStudentAndClassAsync(int studentId, int classId)
        //{
        //    var studentClass = await _dbContext.StudentClasses
        //                                .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.ClassId == classId);
        //    if (studentClass == null)
        //    {
        //        return false;
        //    }
        //    var programClass = await _dbContext.Classes.FindAsync(classId);
        //    var trainingProgram = await _dbContext.TrainingPrograms.FindAsync(programClass.ProgramId);
        //    var certificate = await _dbContext.Certificates.FirstOrDefaultAsync(cer => cer.CertificateType == trainingProgram.ProgramName);
        //    var certificateExist = await _dbContext.StudentCertificates.FirstOrDefaultAsync(c => c.StudentId == studentClass.StudentId && c.CertificateId == certificate.Id);
        //    if (certificateExist != null)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        public async Task<bool> CheckCertificateByStudentAndClassAsync(int studentId, int classId)
        {
            var certificateExist = await _dbContext.StudentCertificates
                .AnyAsync(c => c.StudentId ==
                    _dbContext.StudentClasses
                        .Where(sc => sc.StudentId == studentId && sc.ClassId == classId)
                        .Select(sc => sc.StudentId).SingleOrDefault()
                        &&
                    c.CertificateId ==
                    _dbContext.Certificates
                        .Where(cer => cer.CertificateType ==
                            _dbContext.TrainingPrograms
                                .Where(tp => tp.Id ==
                                    _dbContext.Classes
                                        .Where(cl => cl.Id == classId)
                                        .Select(cl => cl.ProgramId)
                                        .FirstOrDefault())
                                .Select(tp => tp.ProgramCode)
                                .SingleOrDefault())
                        .Select(cer => cer.Id)
                        .FirstOrDefault());
            if (certificateExist)
            {
                return true;
            }
            return false;
        }



        public async Task<bool> AddCertificateToStudentAsync(int studentId, int certificateId, string studentCertificateContent)
        {
            try
            {
                var studentExists = await _dbContext.Students.FindAsync(studentId);
                var certificateExists = await _dbContext.Certificates.FindAsync(certificateId);

                if (studentExists == null || studentExists.IsDelete == true || certificateExists == null || certificateExists.IsDelete == true)
                {
                    return false;
                }
                var random = new Random();
                var certificateCode = random.Next(10000000, 99999999).ToString();
                var providedDate = _currentTime.GetCurrentTime();
                var studentCertificate = new StudentCertificate
                {
                    StudentId = studentId,
                    CertificateId = certificateId,
                    CertificateCode = certificateCode,
                    Content = studentCertificateContent,
                    ProvidedDate = providedDate,
                };
                studentCertificate.CreatedBy = _claimsService.GetCurrentUserId.ToString();
                studentCertificate.CreatedDate = _currentTime.GetCurrentTime();
                await _dbContext.StudentCertificates.AddAsync(studentCertificate);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
