using AutoMapper;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.ViewModels.EmailSendsModels;
using FAMS_GROUP2.Repositories.ViewModels.ModuleModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Repositories
{
    public class EmailSendRepository : GenericRepository<EmailSend>, IEmailSendRepository
    {
        private readonly FamsDbContext _context;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly ICurrentTime _currentTime;

        public EmailSendRepository(FamsDbContext context, IMapper mapper, IClaimsService claimsService,
            ICurrentTime timeService) : base(context, timeService, claimsService)
        {
            _context = context;
            _mapper = mapper;
            _claimsService = claimsService;
            _currentTime = timeService;
        }

        public async Task<List<EmailSend>> GetAllEmailSendFilterBySendDate(PaginationParameter paginationParameter, EmailSendsFilterModule emailSendsFilterModule)
        {
            var query = _context.EmailSends.AsQueryable();

            if (emailSendsFilterModule.StartDate.HasValue)
            {
                query = query.Where(e => e.SendDate >= emailSendsFilterModule.StartDate);
            }

            if (emailSendsFilterModule.EndDate.HasValue)
            {
                query = query.Where(e => e.SendDate <= emailSendsFilterModule.EndDate);
            }

            // Pagination
            var result = await query
                .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                .Take(paginationParameter.PageSize)
                .ToListAsync();

            return result;
        }
         public async Task<List<EmailSend>> CheckIdTemplate(int id)
        {
            var email = _context.EmailSends.Where(x=> x.TemplateId == id).ToList();
            return await Task.FromResult(email);
        }

        public async Task<List<Student>> CheckId(List<int> id)
        {
            var students = await _context.Students.ToListAsync();
            return students.Where(x => id.Any(s => s == x.Id)).ToList();
        }
    }
}
