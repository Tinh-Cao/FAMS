using Application.ViewModels.ResponseModels;
using AutoMapper;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.ViewModels.ScoreModels;
using FAMS_GROUP2.Repositories.ViewModels.StudentModels;
using FAMS_GROUP2.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using System.Security.AccessControl;

namespace FAMS_GROUP2.Services.Services
{
    public class ScoreService : IScoreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ScoreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void CheckAndUpdateScore(object model)
        {
            dynamic scoreModel = model;
            // Kiểm tra điểm từ Quiz1 đến Quiz6
            if (scoreModel.Quiz1 != null && scoreModel.Quiz2 != null && scoreModel.Quiz3 != null && scoreModel.Quiz4 != null && scoreModel.Quiz5 != null && scoreModel.Quiz6 != null)
            {
                scoreModel.QuizAvg = Math.Round((scoreModel.Quiz1 + scoreModel.Quiz2 + scoreModel.Quiz3 + scoreModel.Quiz4 + scoreModel.Quiz5 + scoreModel.Quiz6) / 6, 2);
            }
            else
            {
                scoreModel.QuizAvg = null;
            }

            // Kiểm tra điểm từ Asm1 đến Asm5
            if (scoreModel.Asm1 != null && scoreModel.Asm2 != null && scoreModel.Asm3 != null && scoreModel.Asm4 != null && scoreModel.Asm5 != null)
            {
                scoreModel.AsmAvg = Math.Round((scoreModel.Asm1 + scoreModel.Asm2 + scoreModel.Asm3 + scoreModel.Asm4 + scoreModel.Asm5) / 5, 2);
            }
            else
            {
                scoreModel.AsmAvg = null;
            }

            // Kiểm tra điểm cần thiết để tính GpaModule
            if (scoreModel.QuizAvg != null && scoreModel.QuizFinal != null && scoreModel.AsmAvg != null && scoreModel.PracticeFinal != null && scoreModel.Audit != null)
            {
                scoreModel.Gpamodule = Math.Round((scoreModel.QuizAvg + scoreModel.QuizFinal + scoreModel.AsmAvg + scoreModel.PracticeFinal + scoreModel.Audit) / 5, 2);
            }
            else
            {
                scoreModel.Gpamodule = null;
            }

            if (scoreModel.Quiz1 < 4 || scoreModel.Quiz2 < 4 || scoreModel.Quiz3 < 4 || scoreModel.Quiz4 < 4 || scoreModel.Quiz5 < 4 || scoreModel.Quiz6 < 4 || scoreModel.QuizFinal < 4 || scoreModel.QuizAvg < 6 ||
                scoreModel.Asm1 < 4 || scoreModel.Asm2 < 4 || scoreModel.Asm3 < 4 || scoreModel.Asm4 < 4 || scoreModel.Asm5 < 4 || scoreModel.AsmAvg < 6 || scoreModel.PracticeFinal < 4)
            {
                scoreModel.Status = ScoreStatus.FAILED.ToString();
            }
            else if (scoreModel.Quiz1 == null || scoreModel.Quiz2 == null || scoreModel.Quiz3 == null || scoreModel.Quiz4 == null || scoreModel.Quiz5 == null || scoreModel.Quiz6 == null || scoreModel.QuizFinal == null || scoreModel.QuizAvg == null ||
                scoreModel.Asm1 == null || scoreModel.Asm2 == null || scoreModel.Asm3 == null || scoreModel.Asm4 == null || scoreModel.Asm5 == null || scoreModel.AsmAvg == null || scoreModel.PracticeFinal == null)
            {
                scoreModel.Status = null;
            }
            else
            {
                scoreModel.Status = ScoreStatus.PASS.ToString();
            }

            if (scoreModel.Gpamodule == null)
            {
                scoreModel.LevelModule = null;
            }
            else if (scoreModel.Gpamodule >= 9)
            {
                scoreModel.LevelModule = 1;
            }
            else if (scoreModel.Gpamodule >= 7 && scoreModel.Gpamodule < 9)
            {
                scoreModel.LevelModule = 2;
            }
            else
            {
                scoreModel.LevelModule = 3;
            }
        }

        public async Task<Pagination<ScoreViewModel>> GetScoresByFiltersAsync(PaginationParameter paginationParameter, ScoreFilterModel scoreFilterModel)
        {
            var scores = await _unitOfWork.ScoreRepository.GetScoresByFiltersAsync(paginationParameter, scoreFilterModel);
            var mappedResult = new List<ScoreViewModel>();
            if (scores != null)
            {
                foreach (var model in scores)
                {
                    var mappedModel = _mapper.Map<ScoreViewModel>(model);
                    mappedModel.FullName = await _unitOfWork.ScoreRepository.GetName((int)model.StudentId);
                    mappedResult.Add(mappedModel);
                }
                return new Pagination<ScoreViewModel>(mappedResult, scores.TotalCount, scores.CurrentPage, scores.PageSize);
            }
            return null;
        }

        public async Task<ScoreViewModel> GetScoreByIdAsync(int StudentId, int ClassId)
        {
            var score = await _unitOfWork.ScoreRepository.GetAllAsync();
            var scoreByStudent = score.FirstOrDefault(s => s.StudentId == StudentId && s.ClassId == ClassId);

            if (scoreByStudent != null)
            {
                var result = _mapper.Map<ScoreViewModel>(scoreByStudent);

                return result;
            }

            return null;
        }

        public async Task<ResponseModel> AddScoreByFormAsync(ScoreCreateModel scoreModel)
        {
            var isScoreExisted = await _unitOfWork.ScoreRepository.GetScoreIdAsync(scoreModel.StudentId, scoreModel.ClassId);
            if (isScoreExisted != null)
            {
                return new ResponseModel { Status = false, Message = "Score is already existed" };
            }
            else
            {
                CheckAndUpdateScore(scoreModel);
                var scoreObj = _mapper.Map<Score>(scoreModel);

                var addedScore = await _unitOfWork.ScoreRepository.AddAsync(scoreObj);
                await _unitOfWork.SaveChangeAsync();
                if (addedScore != null)
                {
                    return new ResponseModel { Status = true, Message = "Add Score Successfully!!" };
                }

                return new ResponseModel { Status = false, Message = "Error adding Score" };
            }
        }

        public async Task<ResponseModel> UpdateScoreByStudentAsync(int StudentId, int ClassId, ScoreUpdateModel scoreModel)
        {
            try
            {
                var existingScore = await _unitOfWork.ScoreRepository.GetScoreIdAsync(StudentId, ClassId);
                if (existingScore == null)
                {
                    return new ResponseModel { Status = false, Message = "Invalid ID" };
                }
                else
                {
                    CheckAndUpdateScore(scoreModel);
                    _mapper.Map(scoreModel, existingScore);
                    await _unitOfWork.ScoreRepository.Update(existingScore);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseModel { Status = true, Message = "Score updated successfully" };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel { Status = false, Message = $"Error updating Score: {ex.Message}" };
            }
        }

        public async Task<ResponseModel> UpdateScoreByClassAsync(List<ScoreCreateModel> scoreModel)
        {
            var errorMessages = new List<string>();

            try
            {
                var studentIdList = scoreModel.Select(x => x.StudentId).ToList();
                var classIdList = scoreModel.Select(x => x.ClassId).ToList();
                var scoreExist = await _unitOfWork.ScoreRepository.GetScoreIds(studentIdList, classIdList);
                foreach (var score in scoreModel)
                {
                    var scoreNotExist = scoreExist.Any(s => s.ClassId == score.ClassId && s.StudentId == score.StudentId);
                    if (!scoreNotExist)
                    {
                        errorMessages.Add($"StudentId {score.StudentId} and ClassId {score.ClassId} not found!");
                    }
                }

                foreach (var score in scoreExist)
                {
                    var scoreFound = scoreModel.Find(x => x.StudentId == score.StudentId && x.ClassId == score.ClassId);
                    CheckAndUpdateScore(scoreFound);
                    _mapper.Map(scoreFound, score);
                }

                await _unitOfWork.ScoreRepository.UpdateRange(scoreExist);
                var result = await _unitOfWork.SaveChangeAsync();


                if (errorMessages.Any())
                {
                    return new ResponseModel { Status = true, Message = "Score failed to update: " + string.Join("; ", errorMessages) };
                }
                else
                {
                    return new ResponseModel { Status = true, Message = "Scores updated successfully" };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel { Status = false, Message = $"Error updating Scores: {ex.Message}" };
            }
        }

        public async Task<ResponseModel> DeleteScoreAsync(int StudentId, int ClassId)
        {
            var ScoreExisted = await _unitOfWork.ScoreRepository.GetScoreIdAsync(StudentId, ClassId);
            if (ScoreExisted != null)
            {
                await _unitOfWork.ScoreRepository.SoftRemove(ScoreExisted);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseModel { Status = true, Message = "Score deleted successfully" };
            }
            else
            {
                return new ResponseModel { Status = false, Message = "Score is not existed!!" };
            }
        }

        public async Task<ResponseModel> DeleteScoresByClassAsync(List<int> StudentIds, int ClassId)
        {
            var errorMessages = new List<string>();

            try
            {
                var ScoreExisted = await _unitOfWork.ScoreRepository.GetScoreIdsV2(StudentIds, ClassId);
                foreach (var studentId in StudentIds)
                {
                    var scoreNotExist = !ScoreExisted.Any(s => s.ClassId == ClassId && s.StudentId == studentId);
                    if (scoreNotExist)
                    {
                        errorMessages.Add($"StudentId {studentId} and ClassId {ClassId} not found!");
                    }
                }

                foreach (var score in ScoreExisted)
                {
                    await _unitOfWork.ScoreRepository.SoftRemove(score);
                }

                await _unitOfWork.SaveChangeAsync();

                if (errorMessages.Any())
                {
                    return new ResponseModel { Status = true, Message = "Score failed to delete: " + string.Join("; ", errorMessages) };
                }
                else
                {
                    return new ResponseModel { Status = true, Message = "Scores delete successfully" };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel { Status = false, Message = $"Error delete Scores: {ex.Message}" };
            }
        }

        public async Task<ScoreImportResponseModel> ScoreImportExcel(List<ScoreImportModel> scoreModel)
        {
            var existingScores = new List<ScoreImportModel>();
            var scoresToAdd = new List<Score>();

            
            try
            {
                // Get all existing scores in one query
                var scoresFound = await _unitOfWork.ScoreRepository.GetScoreIdsV1(scoreModel);

                var studentIds =  await _unitOfWork.ScoreRepository.GetStudents(scoreModel);

                foreach (var score in scoreModel)
                {
                    var scoreExist = scoresFound.Any(s => s.Student.StudentCode == score.StudentCode && s.ClassId == score.ClassId);

                    if (scoreExist)
                    {
                        existingScores.Add(score);
                    }
                    else
                    {
                        CheckAndUpdateScore(score);
                        var scoreObj = _mapper.Map<Score>(score);
                        scoreObj.StudentId = studentIds.Where(s => s.StudentCode == score.StudentCode).Select(s => s.Id).FirstOrDefault();
                        scoresToAdd.Add(scoreObj);
                    }
                }

                // Add all new scores in one query
                await _unitOfWork.ScoreRepository.AddRangeAsync(scoresToAdd);
                await _unitOfWork.SaveChangeAsync();

                return new ScoreImportResponseModel
                {
                    Status = true,
                    Message = "Scores added successfully.",
                    ExistingScores = existingScores.ToList(),
                };
            }
            catch (Exception ex)
            {
                return new ScoreImportResponseModel
                {
                    Status = false,
                    Message = "Error: " + ex.Message,
                    ExistingScores = existingScores
                };
            }
        }

    }
}