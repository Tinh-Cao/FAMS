using Application.ViewModels.ResponseModels;
using AutoMapper;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.ViewModels.EmailModel;
using FAMS_GROUP2.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FAMS_GROUP2.Services.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public MailService(IOptions<MailSettings> mailSettings, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mailSettings = mailSettings.Value;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseModel> SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                var studentFound = await _unitOfWork.EmailSendRepository.CheckId(mailRequest.StudentIds);

                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);

                if (studentFound.Any())
                {
                    foreach (var student in studentFound)
                    {
                        email.To.Add(MailboxAddress.Parse(student.Email));
                    }
                }

                email.Subject = mailRequest.Subject;
                var builder = new BodyBuilder();
                if (mailRequest.Attachments != null)
                {
                    byte[] filebytes;
                    foreach (var file in mailRequest.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                filebytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.Name, filebytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }
                builder.HtmlBody = mailRequest.Body;
                email.Body = builder.ToMessageBody();
                using var smpt = new SmtpClient();
                smpt.Connect(_mailSettings.Host, int.Parse(_mailSettings.Port), SecureSocketOptions.StartTls);
                smpt.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smpt.SendAsync(email);
                smpt.Disconnect(true);

                var sendmail = _mapper.Map<EmailSend>(mailRequest);


                sendmail.SendDate = DateTime.UtcNow;

                sendmail.Subject = mailRequest.Subject;
                sendmail.Content = mailRequest.Body;


                await _unitOfWork.EmailSendRepository.AddAsync(sendmail);
                await _unitOfWork.SaveChangeAsync();
                var receiveMails = new List<EmailSendStudent>();
                foreach (var student in studentFound)
                {
                    var receiveMail = new EmailSendStudent
                    {
                        ReceiveId = student.Id,
                        EmailSendId = sendmail.Id,
                    };

                    receiveMails.Add(receiveMail);
                }
                await _unitOfWork.EmailSendStudentRepository.AddRangeAsync(receiveMails);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseModel { Status = true, Message = "Send Mail Successfully!" };
            }
            catch (Exception ex)
            {
                if(ex.InnerException != null)
                {
                    return new ResponseModel
                    {
                        Message = ex.InnerException.Message,
                    };
                }
                return new ResponseModel
                {
                    Message = ex.Message
                };
            }

        }
    }
}
