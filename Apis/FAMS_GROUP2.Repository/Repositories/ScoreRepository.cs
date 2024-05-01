using Application.ViewModels.ResponseModels;
using AutoMapper;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.ModuleModels;
using FAMS_GROUP2.Repositories.ViewModels.ScoreModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Org.BouncyCastle.Crypto;

namespace FAMS_GROUP2.Repositories.Repositories
{
    public class ScoreRepository : GenericRepository<Score>, IScoreRepository
    {
        private readonly FamsDbContext _db;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;

        public ScoreRepository(FamsDbContext db, ICurrentTime currentTime, IClaimsService claimsService) : base(db, currentTime, claimsService)
        {
            _db = db;
            _currentTime = currentTime;
            _claimsService = claimsService;
        }

        public async Task<Score> GetScoreIdAsync(int studentId, int classId)
        {
            var score = await _db.Scores.FirstOrDefaultAsync(x => x.StudentId == studentId && x.ClassId == classId);
            return score;
        }

        public async Task<List<Score>> GetScoreIds(List<int> StudentIds, List<int> ClassId)
        {
            var pair = StudentIds.Zip(ClassId, (studentId, classId) => new { StudentId = studentId, ClassId = classId });
            var result = await _db.Scores.ToListAsync();
            return result.Where(s => pair.Any(pair => pair.StudentId == s.StudentId && pair.ClassId == s.ClassId)).ToList();
        }

        public async Task<List<Score>> GetScoreIdsV1(List<ScoreImportModel> scores)
        {
            //var pair = StudentCodes.Zip(ClassId, (studentCode, classId) => new { StudentCode = studentCode, ClassId = classId });
            var result = await _db.Scores.Include(s => s.Student).ToListAsync();
            //return result.Where(s => pair.Any(pair => pair.StudentCode == s.Student.StudentCode && pair.ClassId == s.ClassId)).ToList();
            return result.Where(s => scores.Any(a => a.StudentCode == s.Student?.StudentCode && a.ClassId == s.ClassId)).ToList();
        }

        public async Task<List<Score>> GetScoreIdsV2(List<int> StudentIds, int ClassId)
        {
            return await _db.Scores.Where(s => StudentIds.Contains((int)s.StudentId) && s.ClassId == ClassId).ToListAsync();
        }

        public async Task<string> GetName(int studentId)
        {
            var score = await _db.Scores.FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (score != null)
            {
                var student = await _db.Students.FirstOrDefaultAsync(s => s.Id == score.StudentId);
                if (student != null)
                {
                    return student.FullName;
                }
            }
            return null;
        }

        public async Task<Pagination<Score>> GetScoresByFiltersAsync(PaginationParameter paginationParameter, ScoreFilterModel scoreFilterModel)
        {
            try
            {
                var scoresQuery = _db.Scores.AsQueryable();
                scoresQuery = await ApplyFilterSortAndSearch(scoresQuery, scoreFilterModel);
                if (scoresQuery != null)
                {
                    var sortedQuery = ApplySorting(scoresQuery, scoreFilterModel);
                    var totalCount = await sortedQuery.CountAsync();

                    var scoresPagination = await sortedQuery
                        .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                        .Take(paginationParameter.PageSize)
                        .ToListAsync();
                    return new Pagination<Score>(scoresPagination, totalCount, paginationParameter.PageIndex, paginationParameter.PageSize);
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<IQueryable<Score>> ApplyFilterSortAndSearch(IQueryable<Score> query, ScoreFilterModel scoreFilterModel)
        {
            if (scoreFilterModel.IsDelete == true)
            {
                query = query.Where(a => a.IsDelete == true);
            }
            else if (scoreFilterModel.IsDelete == false)
            {
                query = query.Where(a => a.IsDelete == false);
            }
            else
            {
                query = query.Where(a => a.IsDelete == true || a.IsDelete == false);
            }

            if (scoreFilterModel.ClassId != null)
            {
                query = query.Where(a => a.ClassId == scoreFilterModel.ClassId);
            }

            if (!string.IsNullOrEmpty(scoreFilterModel.Status))
            {
                query = query.Where(a => a.Status == scoreFilterModel.Status);
            }

            if (scoreFilterModel.LevelModule != null)
            {
                query = query.Where(a => a.LevelModule == scoreFilterModel.LevelModule);
            }

            if (!string.IsNullOrEmpty(scoreFilterModel.Search))
            {
                query = query.Where(a => a.Student.FullName.Contains(scoreFilterModel.Search));
            }
            return query;
        }

        private IQueryable<Score> ApplySorting(IQueryable<Score> query, ScoreFilterModel scoreFilterModel)
        {
            switch (scoreFilterModel.Sort.ToLower())
            {
                case "Status":
                    query = (scoreFilterModel.SortDirection.ToLower() == "desc") ? query.OrderByDescending(a => a.Status) : query.OrderBy(a => a.Status);
                    break;
                case "LevelModule":
                    query = (scoreFilterModel.SortDirection.ToLower() == "desc") ? query.OrderByDescending(a => a.LevelModule) : query.OrderBy(a => a.LevelModule);
                    break;
                default:
                    query = (scoreFilterModel.SortDirection.ToLower() == "desc") ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id);
                    break;
            }

            return query;
        }

        public async Task<List<Student>> GetStudents (List<ScoreImportModel> scores)
        {
            var result = await _db.Students.ToListAsync();
            return result.Where(s => scores.Any(st => st.StudentCode == s.StudentCode)).ToList();
        }

    }
}