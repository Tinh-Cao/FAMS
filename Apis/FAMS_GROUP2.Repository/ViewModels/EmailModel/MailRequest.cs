using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.ViewModels.EmailModel
{
    public class MailRequest
    {
        public List<int> StudentIds { get; set; }
        public int SenderId { get; set; }
        public int TemplateId { get; set; }
        //public string  ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile>? Attachments { get; set; }


    }
}
