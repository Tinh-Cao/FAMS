using Application.ViewModels.ResponseModels;
using AutoMapper;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.LessonModels;
using FAMS_GROUP2.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Services.Services
{
    public class LessonService : ILessonService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public LessonService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseModel> CreateLessonAsync(CreateLessonModel lessonModel)
        {
            var lessonObj = _mapper.Map<Lesson>(lessonModel);
            var addLesson = await _unitOfWork.LessonRepository.AddAsync(lessonObj);
            await _unitOfWork.SaveChangeAsync();
            if (addLesson != null)
            {
                return new ResponseModel { Status = true, Message = "Added Lesson Successfully" };
            }
            return new ResponseModel { Status = false, Message = "Error adding!!" };
        }

        public async Task<List<Lesson>> GetAllLessonAsync()
        {
            var lessGetAll = await _unitOfWork.LessonRepository.GetAllAsync();
            var lessMapper = _mapper.Map<List<Lesson>>(lessGetAll);
            return lessMapper;
        }
        public async Task<Lesson> GetLessonByIdAsync(int id)
        {
            var lessFound = await _unitOfWork.LessonRepository.GetByIdAsync(id);
            if (lessFound != null)
            {
                return lessFound;
            }
            var lessMapper = _mapper.Map<Lesson>(lessFound);
            return lessMapper;
        }
        public async Task<ResponseModel> UpdateLessonAsync(int id, UpdateLessonModel lesson)
        {
            var lessUpdate = await _unitOfWork.LessonRepository.GetByIdAsync(id);
            if (lessUpdate != null)
            {
                lessUpdate = _mapper.Map(lesson, lessUpdate);
                await _unitOfWork.LessonRepository.Update(lessUpdate);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    return new ResponseModel
                    {
                        Status = true,
                        Message = "Lesson update successfully!"
                    };
                }
                return new ResponseModel
                {
                    Status = false,
                    Message = "Lesson update failed!"
                };
            }
            return new ResponseModel
            {
                Status = false,
                Message = "Lesson not found!"
            };
        }
        public async Task<ResponseModel> DeleteLessonAsync(int id)
        {   
            var deleteLesson = await _unitOfWork.LessonRepository.GetByIdAsync(id);
            if (deleteLesson != null)
            {
                await _unitOfWork.LessonRepository.SoftRemove(deleteLesson);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseModel { Status = true, Message = "Lesson deleted successfully" };
            }
            return new ResponseModel { Status = false, Message = "Lesson deleted false" };
        }
        public async Task<Pagination<LessonDetailsModel>> GetLessonByFilterAsync(PaginationParameter paginationParameter, LessonFilterModel lessonFilterModel)
        {
            try
            {
                var lessons = await _unitOfWork.LessonRepository.GetLessonByFilterAsync(paginationParameter, lessonFilterModel);
                if (lessons != null)
                {
                    var mapperResult = _mapper.Map<List<LessonDetailsModel>>(lessons);
                    return new Pagination<LessonDetailsModel>(mapperResult, lessons.TotalCount, lessons.CurrentPage, lessons.PageSize);
                }
                return null; 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }   
}
