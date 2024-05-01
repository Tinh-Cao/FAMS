using AutoMapper;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.Repositories;
using FAMS_GROUP2.Repositories.ViewModels.EmailSendsModels;
using FAMS_GROUP2.Repositories.ViewModels.ModuleModels;
using FAMS_GROUP2.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Services.Services
{
    public class EmailSendService : IEmailSendService
    {
        public readonly IEmailSendRepository _emailSendReponsitory;
        private readonly IMapper _mapper;
        private readonly FamsDbContext _dbcontext;
        private readonly IUnitOfWork _unitOfWork;

        public EmailSendService(IEmailSendRepository emailSend, IMapper mapper, FamsDbContext context, IUnitOfWork unitOfWork)
        {
            _emailSendReponsitory = emailSend;
            _mapper = mapper;
            _dbcontext = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Pagination<EmailSend>> GetAllEmailSendFilterBySendDateAsync(PaginationParameter paginationParameter, EmailSendsFilterModule emailSendsFilterModule)
        {
            var emailQuery = await _emailSendReponsitory.GetAllEmailSendFilterBySendDate(paginationParameter, emailSendsFilterModule);
            var count = emailQuery.Count();
            var mapmodules = _mapper.Map<List<EmailSend>>(emailQuery).ToList();
            var paginationResult = new Pagination<EmailSend>(mapmodules, count, paginationParameter.PageIndex, paginationParameter.PageSize);

            return paginationResult;
        }
      
        public async Task<EmailSend> GetSendMailByIdAsync(int id)
        {
            var result = await _unitOfWork.EmailSendRepository.GetByIdAsync(id);
            if (result != null)
            {
                return result;
            }

            return null;
        }
    }
}
