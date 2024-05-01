using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.ViewModels.DocumentModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FAMS_GROUP2.Repositories.Repositories
{
    public class DocumentRepository : GenericRepository<Document>, IDocumentRepository
    {
        private readonly FamsDbContext _dbContext;
        public DocumentRepository(FamsDbContext context,
            ICurrentTime timeService,
            IClaimsService claimService)
            : base(context, timeService, claimService)
        {
            _dbContext = context;
        }
        public async Task<Pagination<Document>> GetDocumentFilterAsync(PaginationParameter paginationParameter, DocumentFilterModel documentFilterModel)
        {
            try
            {
                var documentsQuery = _dbContext.Documents.AsQueryable();
                documentsQuery = await ApplyFilterSortAndSearch(documentsQuery, documentFilterModel);
                if (documentsQuery != null)
                {
                    var documentQuery = ApplySorting(documentsQuery, documentFilterModel);
                    var totalCount = await documentQuery.CountAsync();

                    var documentPagination = await documentQuery
                        .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                        .Take(paginationParameter.PageSize)
                        .ToListAsync();
                    return new Pagination<Document>(documentPagination, totalCount, paginationParameter.PageIndex, paginationParameter.PageSize);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<IQueryable<Document>> ApplyFilterSortAndSearch(IQueryable<Document> Query, DocumentFilterModel documentFilterModel)
        {
            if (documentFilterModel == null)
            {
                return Query;
            }
            if (!string.IsNullOrEmpty(documentFilterModel.Search))
            {
                Query = Query.Where(x => x.DocumentName.Contains(documentFilterModel.Search));
            }
            if (documentFilterModel.LessonId != null)
            {
                Query = Query.Where(asm => asm.LessonId == documentFilterModel.LessonId);
            }
            return Query;
        }
        private IQueryable<Document> ApplySorting(IQueryable<Document> query, DocumentFilterModel documentFilterModel)
        {
            switch (documentFilterModel.Sort.ToLower())
            {
                case "documentname":
                    query = (documentFilterModel.SortDirection.ToLower() == "desc") ? query.OrderByDescending(x => x.DocumentName) : query.OrderBy(x => x.DocumentName);
                    break;
                default:
                    query = (documentFilterModel.SortDirection.ToLower() == "desc") ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id);
                    break;
            }
            return query;
        }
    }
}
