using AutoMapper;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.ViewModels.EmailModel;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Repositories
{
    public class EmailTemplateRepository : GenericRepository<EmailTemplate>, IEmailTemplateRepository
    {
        private readonly FamsDbContext _context;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly ICurrentTime _currentTime;
        public EmailTemplateRepository(FamsDbContext context,
              ICurrentTime timeService,
              IClaimsService claimsService, IMapper mapper)
              : base(context, timeService, claimsService)
        {
            _context = context;
            _claimsService = claimsService;
            _currentTime = timeService;
             _mapper = mapper;
        }



        public async Task<EmailTemplate> BanEmailTemplate(int id)
        {
            var emailTemplate = await _context.EmailTemplates.FindAsync(id);

            if (emailTemplate == null)
            {
                // Handle the case where the email template with the given ID is not found
                return null;
            }
            emailTemplate.IsDelete = true;
            emailTemplate.DeletedDate = _currentTime.GetCurrentTime();
            emailTemplate.DeletedBy = _claimsService.GetCurrentUserId.ToString();

            await _context.SaveChangesAsync();

            return emailTemplate;

        }

    

        public async Task<EmailTemplate> UnBanEmailTemplate(int id)
        {
            var emailTemplate = await _context.EmailTemplates.FindAsync(id);

            if (emailTemplate == null)
            {
                // Handle the case where the email template with the given ID is not found
                return null;
            }
            emailTemplate.IsDelete = false;
            emailTemplate.DeletedDate = _currentTime.GetCurrentTime();
            emailTemplate.DeletedBy = _claimsService.GetCurrentUserId.ToString();

            await _context.SaveChangesAsync();

            return emailTemplate;
        }

        public async Task<EmailTemplate> UpdateEmailTemplate(int id, EmailTemplateModel model)
        {
            var emailTemplate = await _context.EmailTemplates.FindAsync(id);

            if (emailTemplate == null)
            {
                // Handle the case where the email template with the given ID is not found
                return null;
            }

            // Update properties based on the provided model
            emailTemplate.Type = model.Type;
            emailTemplate.Name = model.Name;
            emailTemplate.Content = model.Content;         
            emailTemplate.ModifiedDate = _currentTime.GetCurrentTime();
            emailTemplate.ModifiedBy = _claimsService.GetCurrentUserId.ToString();

            // Save changes to the database
            await _context.SaveChangesAsync();

            return emailTemplate;
        }

        public async Task DeleteAsync(int id)
        {
            var delete = _context.EmailTemplates.SingleOrDefault(b => b.Id == id);
            if (delete != null)
            {
                _context.EmailTemplates.Remove(delete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
