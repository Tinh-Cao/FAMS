using Application.ViewModels.ResponseModels;
using AutoMapper;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.ViewModels.EmailModel;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Services.Interfaces;

namespace FAMS_GROUP2.Services.Services
{
    public class EmailTemplateServices : IEmailTemplateServices
    {
        public readonly IEmailTemplateRepository _templateReponsitory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EmailTemplateServices(IEmailTemplateRepository templateReponsitory, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _templateReponsitory = templateReponsitory;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

      

        public async Task<EmailTemplate> AddEmailTemplateAsync(EmailTemplateModel templateModel)
        {

            var emailTemplate = _mapper.Map<EmailTemplate>(templateModel);
            var addEmT = await _unitOfWork.EmailTemplateRepository.AddAsync(emailTemplate);
            await _unitOfWork.SaveChangeAsync();
            if (addEmT != null)
            {
                return addEmT;
            }
            return null;
        }

        public async Task<EmailTemplate> BanEmailTemplateAsync(int id)
        {
            return await _templateReponsitory.BanEmailTemplate(id);
        }



        public async Task<List<EmailTemplate>> GetAllEmailTemplateAsync()
        {
            return await _unitOfWork.EmailTemplateRepository.GetAllAsync();
        }

        public async Task<EmailTemplate> GetAllEmailTemplateByIdAsync(int id)
        {
            var result = await _unitOfWork.EmailTemplateRepository.GetByIdAsync(id);
            if (result != null)
            {
                return result;
            }

            return null;
        }

        public async Task<EmailTemplate> UnBanEmailTemplateAsync(int id)
        {
            return await _templateReponsitory.UnBanEmailTemplate(id);
        }

        public async Task<EmailTemplate> UpdateEmailTemplateAsync(int id, EmailTemplateModel model)
        {
            return await _templateReponsitory.UpdateEmailTemplate(id, model);
        }
        public async Task<ResponseModel> DeleteEmailTemplateAsync(int id)
        {
            try
            {
                var emailTemplate = await _unitOfWork.EmailTemplateRepository.GetByIdAsync(id);
                var email = await _unitOfWork.EmailSendRepository.CheckIdTemplate(id);
                if (emailTemplate == null)
                {
                    return new ResponseModel { Status = false, Message = "Not found." };
                }

                if (email.Any())
                {
                    return new ResponseModel { Status = false, Message = " EmailTemplate using." };
                }

                await _unitOfWork.EmailTemplateRepository.DeleteAsync(id);
                await _unitOfWork.SaveChangeAsync();


                return new ResponseModel { Status = true, Message = "remove successfull." };
            }
            catch (Exception ex)
            {
                return new ResponseModel { Status = false, Message = "Đã xảy ra lỗi : " + ex.Message };
            }
        }
    }
}
