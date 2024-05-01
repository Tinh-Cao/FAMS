using Application.ViewModels.ResponseModels;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.DocumentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Services.Interfaces
{
    public interface IDocumentService
    {
        public Task<ResponseModel> CreateDocumentAsync(CreateDocumentModel documentModel);
        public Task<List<Document>> GetAllDocumentAsync();
        public Task<Document> GetDocumentByIdAsync(int id);
        public Task<ResponseModel> UpdateDocumentAsync(int id, UpdateDocumentModel document);
        public Task<ResponseModel> DeleteDocumentAsync(int id);
        public Task<Pagination<DocumentDetailsModel>> GetDocumentFilterAsync(PaginationParameter paginationParameter, DocumentFilterModel documentFilterModel);
    }
}
