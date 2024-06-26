﻿using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Repositories
{
    public class EmailSendStudentRepository : GenericRepository<EmailSendStudent>, IEmailSendStudentRepository
    {
        public EmailSendStudentRepository(FamsDbContext context,
       ICurrentTime timeService,
       IClaimsService claimsService)
       : base(context, timeService, claimsService)
        {


        }
    }
}
