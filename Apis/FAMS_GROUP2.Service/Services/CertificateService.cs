using Application.ViewModels.ResponseModels;
using AutoMapper;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.ViewModels.CertificateModels;
using FAMS_GROUP2.Services.Interfaces;
using Newtonsoft.Json;

namespace FAMS_GROUP2.Services.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttendanceClassService _attendanceClassService;
        private readonly IStudentService _studentService;
        private readonly IScoreService _scoreService;
        private readonly ICurrentTime _currentTime;

        public CertificateService(IMapper mapper, IAttendanceClassService attendanceClassService, ICurrentTime currentTime,
            IStudentService studentService, IScoreService scoreService, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _attendanceClassService = attendanceClassService;
            _studentService = studentService;
            _scoreService = scoreService;
            _currentTime = currentTime;
        }

        public async Task<ResponseModel> CreateCertificateAsync(CertificateModel certificateCreateModel)
        {
            var checkCertificateType = await _unitOfWork.CertificateRepository.GetCertificateByTypeAsync(certificateCreateModel.CertificateType);
            if (checkCertificateType == null)
            {
                var certificate = _mapper.Map<Certificate>(certificateCreateModel);
                var result = await _unitOfWork.CertificateRepository.AddAsync(certificate);
                await _unitOfWork.SaveChangeAsync();
                if (result != null)
                {
                    return new ResponseModel
                    {
                        Status = true,
                        Message = "Added Successfully"
                    };
                }
                return new ResponseModel
                {
                    Status = false,
                    Message = "Added Unsuccessfully!"
                };
            }
            return new ResponseModel
            {
                Status = false,
                Message = "Type of certificate is already existed"
            }; ;

        }

        public async Task<ResponseModel> UpdateCertificateAsync(int certificateId, CertificateModel certificateUpdateModel)
        {
            var existingCertificate = await _unitOfWork.CertificateRepository.GetByIdAsync(certificateId);
            if (existingCertificate != null)
            {
                existingCertificate = _mapper.Map(certificateUpdateModel, existingCertificate);
                await _unitOfWork.CertificateRepository.Update(existingCertificate);
                await _unitOfWork.SaveChangeAsync();
                var responseModel = new ResponseModel
                {
                    Status = true,
                    Message = "Certificate updated successfully",
                };
                return responseModel;
            }
            return new ResponseModel
            {
                Status = false,
                Message = "Certificate Template is not existed!"
            };
        }

        public async Task<ResponseModel> DeleteCertificateAsync(int certificateId)
        {
            var certificate = await _unitOfWork.CertificateRepository.GetByIdAsync(certificateId);
            if (certificate != null)
            {
                await _unitOfWork.CertificateRepository.SoftRemove(certificate);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseModel
                {
                    Status = true,
                    Message = "Certificate ban successfully!"
                };
            }
            return new ResponseModel
            {
                Status = false,
                Message = "Certificate Template is not existed!"
            };
        }

        public async Task<ResponseModel> UnDeleteCertificateAsync(int certificateId)
        {
            var certificate = await _unitOfWork.CertificateRepository.GetByIdAsync(certificateId);
            if (certificate != null)
            {
                certificate.IsDelete = false;
                await _unitOfWork.CertificateRepository.Update(certificate);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseModel
                {
                    Status = true,
                    Message = "Certificate unbanned successfully!"
                };
            }
            return new ResponseModel
            {
                Status = false,
                Message = " Certificate Template is not existed!"
            };
        }

        public async Task<ResponseModel> ProvideCertificateAsync(CertificateProvideModel certificateModel)
        {
            var responseModel = new ResponseModel();
                var existingCertificate = await _unitOfWork.CertificateRepository.CheckCertificateByStudentAndClassAsync(certificateModel.studentId, certificateModel.classId);
                if (existingCertificate)
                {
                    responseModel.Status = false;
                    responseModel.Message = "Certificate already exists";
                    return responseModel;
                }
                var student = await _unitOfWork.StudentRepository.GetStudentWithScoreAndClassWithTrainingProgramAsync(certificateModel.studentId, certificateModel.classId);
                var trainingProgram = student.StudentClasses.First().Class.Program;
                var studentScore = student.Scores.First();
                var attendanceAvg = await _attendanceClassService.ViewPresentAndAbsentOfStudentInClass(certificateModel.classId, certificateModel.studentId);
                var presencePercent = float.Parse(attendanceAvg.listObject[2].ToString());

                if (student.StudentClasses.First().Class != null && trainingProgram != null && studentScore != null &&
                  presencePercent >= 0.8 && studentScore.Status == "PASS")
                {
                    var certificate = await _unitOfWork.CertificateRepository.GetCertificateByTypeAsync(trainingProgram.ProgramCode);
                    if (certificate != null)
                    {
                        if (certificate.IsDelete == true)
                        {
                            responseModel.Status = false;
                            responseModel.Message = "Certificate was blocked!";
                            return responseModel;
                        }
                        var certificateCourseName = trainingProgram.ProgramName + " " + trainingProgram.ProgramCode;
                        var issueDate = DateTime.Now;
                        var certificateContent = new CertificateContentModel
                        {
                            FullName = student.FullName,
                            GPA = studentScore.Gpamodule.ToString(),
                            IssueDate = issueDate.ToString(),
                            CourseName = certificateCourseName.ToString(),
                        };
                        var contentJson = JsonConvert.SerializeObject(certificateContent);

                        await _unitOfWork.CertificateRepository.AddCertificateToStudentAsync(certificateModel.studentId, certificate.Id, contentJson);
                        await _unitOfWork.SaveChangeAsync();

                        responseModel.Status = true;
                        responseModel.Message = "Provided certificates successfully!";
                        return responseModel;
                    }
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Failed to provide certificates.";
                    return responseModel;
                }
            return responseModel;
        }

        public async Task<CertificateViewModel> GetCertificateAsync(int studentId, int classId)
        {
            var existingCertificate = await _unitOfWork.CertificateRepository.CheckCertificateByStudentAndClassAsync(studentId, classId);

            if (existingCertificate)
            {
                var studentCertificate = await _unitOfWork.CertificateRepository.GetCertificateByStudentIdAsync(studentId);
                var templateContent = await _unitOfWork.CertificateRepository.GetByIdAsync(studentCertificate.CertificateId);
                // Deserialize templateContent thành CertificateContentModel
                var certificateContent = JsonConvert.DeserializeObject<CertificateContentModel>(studentCertificate.Content);
                // Thay thế các placeholder trong content với các giá trị từ certificateContent
                var replacedContent = templateContent.Content
                    .Replace("[FullName]", certificateContent.FullName)
                    .Replace("[GPA]", certificateContent.GPA)
                    .Replace("[IssueDate]", certificateContent.IssueDate)
                    .Replace("[CourseName]", certificateContent.CourseName);
                var certificateViewModel = new CertificateViewModel
                {
                    certificateName = templateContent.CertificateName,
                    certificateCode = studentCertificate.CertificateCode,
                    Content = replacedContent
                };
                return certificateViewModel;
            }
            return null;
        }

        public async Task<List<CertificateViewModel>> GetAllStudentCertificateAsync(int studentId)
        {
            var certificates = await _unitOfWork.CertificateRepository.GetAllCertificatesByStudentIdAsync(studentId);
            if (certificates == null)
            {
                return null;
            }
            var certificateContents = new List<CertificateViewModel>();
            foreach (var certificate in certificates)
            {
                var templateContent = await _unitOfWork.CertificateRepository.GetByIdAsync(certificate.CertificateId);
                // Deserialize templateContent thành CertificateContentModel
                var certificateContent = JsonConvert.DeserializeObject<CertificateContentModel>(certificate.Content);
                // Thay thế các placeholder trong content với các giá trị từ certificateContent
                var replacedContent = templateContent.Content
                    .Replace("[FullName]", certificateContent.FullName)
                    .Replace("[GPA]", certificateContent.GPA)
                    .Replace("[IssueDate]", certificateContent.IssueDate)
                    .Replace("[CourseName]", certificateContent.CourseName);
                var certificateResult = new CertificateViewModel
                {
                    certificateName = templateContent?.CertificateName,
                    certificateCode = certificate.CertificateCode,
                    Content = replacedContent
                };
                certificateContents.Add(certificateResult);
            }
            return certificateContents;
        }

        public async Task<List<CertificateModel>> GetAllCertificateTemplate()
        {
            var certificateTemplate = await _unitOfWork.CertificateRepository.GetAllAsync();

            var certificateDetail = new List<CertificateModel>();
            foreach (var certificate in certificateTemplate)
            {
                var certificateResult = new CertificateModel
                {
                    CertificateName = certificate.CertificateName,
                    CertificateType = certificate.CertificateType.ToString(),
                    Content = certificate.Content
                };

                certificateDetail.Add(certificateResult);
            }
            return certificateDetail;
        }
    }
}
