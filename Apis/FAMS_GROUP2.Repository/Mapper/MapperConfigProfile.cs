using AutoMapper;

using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.ViewModels.StudentModels;
using FAMS_GROUP2.Repositories.ViewModels.AttendanceModels;
using FAMS_GROUP2.Repositories.ViewModels.ModuleModels;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAMS_GROUP2.Repositories.ViewModels.ProgramModels;
using FAMS_GROUP2.Repositories.ViewModels.ScoreModels;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.LessonModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.ViewModels.AssignmentModels;
using FAMS_GROUP2.Repositories.ViewModels.CertificateModels;
using FAMS_GROUP2.Repositories.ViewModels.DocumentModels;
using FAMS_GROUP2.Repositories.ViewModels.EmailModel;

namespace FAMS_GROUP2.Repositories.Mapper
{
    public class MapperConfigProfile : Profile
    {
        public MapperConfigProfile()
        {
            CreateMap<CreateClassModel, Class>();
            CreateMap<UpdateClassModel, Class>();
            CreateMap<Class, ClassItemModel>()
                .ForMember(dest => dest.ProgramName, opt => opt.MapFrom(src => src.Program.ProgramName))
                .ForMember(dest => dest.ProgramCode, opt => opt.MapFrom(src => src.Program.ProgramCode))
                .ForMember(dest => dest.ProgramId, opt => opt.MapFrom(src => src.Program.Id))
                .ForMember(dest => dest.AdminId, opt => opt.MapFrom(src => src.ClassAccounts.FirstOrDefault(ca => ca.ClassId == src.Id).AdminId))
                .ForMember(dest => dest.TrainerId, opt => opt.MapFrom(src => src.ClassAccounts.FirstOrDefault(ca => ca.ClassId == src.Id).TrainerId));

            CreateMap<StudentImportModel, Student>().ReverseMap(); //.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToLower() == "male")).ReverseMap();

            CreateMap<StudentUpdateModel, Student>().
                ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToLower() == "male")).ReverseMap(); // For update mapping
            //CreateMap<StudentUpdateModel, Student>().ReverseMap();

            CreateMap<Student, StudentDetailsModel>().ReverseMap();

            CreateMap<AccountUpdateModel, Account>()
             .ForMember(dest => dest.Role, opt => opt.Ignore())
             .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToLower() == "male")).ReverseMap(); 
            CreateMap<CreateModuleViewModel, Module>().ReverseMap();
            CreateMap<UpdateModuleViewModel, Module>().ReverseMap();
            CreateMap<CreateLessonModel, Lesson>().ReverseMap();
            CreateMap<UpdateLessonModel, Lesson>().ReverseMap();
            CreateMap<Account, AccountDetailsModel>();
            CreateMap<ProgramModel, TrainingProgram>();           
            CreateMap<Module, ModuleDetailsModel>().ReverseMap();
            CreateMap<Score, ScoreCreateModel>().ForMember(dest => dest.QuizAvg, opt => opt.Ignore()).ForMember(dest => dest.AsmAvg, opt => opt.Ignore()).ForMember(dest => dest.Gpamodule, opt => opt.Ignore()).ForMember(dest => dest.LevelModule, opt => opt.Ignore()).ForMember(dest => dest.Status, opt => opt.Ignore()).ReverseMap();
            CreateMap<Score, ScoreUpdateModel>().ForMember(dest => dest.QuizAvg, opt => opt.Ignore()).ForMember(dest => dest.AsmAvg, opt => opt.Ignore()).ForMember(dest => dest.Gpamodule, opt => opt.Ignore()).ForMember(dest => dest.LevelModule, opt => opt.Ignore()).ForMember(dest => dest.Status, opt => opt.Ignore()).ReverseMap();
            CreateMap<Score, ScoreImportModel>().ForMember(dest => dest.QuizAvg, opt => opt.Ignore()).ForMember(dest => dest.AsmAvg, opt => opt.Ignore()).ForMember(dest => dest.Gpamodule, opt => opt.Ignore()).ForMember(dest => dest.LevelModule, opt => opt.Ignore()).ForMember(dest => dest.Status, opt => opt.Ignore()).ReverseMap();
            CreateMap<Score, ScoreViewModel>().ReverseMap();
            CreateMap<Lesson, LessonDetailsModel>().ReverseMap();
            CreateMap<EmailTemplateModel, EmailTemplate>().ReverseMap();
            CreateMap<AssignmentViewModel, Assignment>().ReverseMap();
            CreateMap<AssignmentImportModel, Assignment>().ReverseMap();
            CreateMap<ProgramResponseModel,TrainingProgram>().ReverseMap();
            CreateMap<CertificateModel, Certificate>();
            CreateMap<CertificateUpdateModel, Certificate>().ReverseMap();
            CreateMap<MailRequest, EmailSend>().ReverseMap();
            CreateMap<CreateDocumentModel, Document>().ReverseMap();
            CreateMap<UpdateDocumentModel, Document>().ReverseMap();
            CreateMap<Document, DocumentDetailsModel>().ReverseMap();

        }
    }
}
