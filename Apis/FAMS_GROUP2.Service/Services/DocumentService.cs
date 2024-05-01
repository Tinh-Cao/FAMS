using Application.ViewModels.ResponseModels;
using AutoMapper;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.ViewModels.DocumentModels;
using FAMS_GROUP2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Services.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DocumentService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseModel> CreateDocumentAsync(CreateDocumentModel documentModel)
        {
            var documentObj = _mapper.Map<Document>(documentModel);
            var addDocument = await _unitOfWork.DocumentRepository.AddAsync(documentObj);
            await _unitOfWork.SaveChangeAsync();
            if (addDocument != null)
            {
                return new ResponseModel { Status = true, Message = "Add Document Successfully" };
            }
            return new ResponseModel { Status = false, Message = "Error added!!" };
        }

        public async Task<List<Document>> GetAllDocumentAsync()
        {
            var DocGetAll = await _unitOfWork.DocumentRepository.GetAllAsync();
            var DocMapper = _mapper.Map<List<Document>>(DocGetAll);
            return DocMapper;
        }
        public async Task<Document> GetDocumentByIdAsync(int id)
        {
            var DocFound = await _unitOfWork.DocumentRepository.GetByIdAsync(id);
            if (DocFound != null)
            {
                return DocFound;
            }
            var DocMapper = _mapper.Map<Document>(DocFound);
            return DocMapper;
        }
        public async Task<ResponseModel> UpdateDocumentAsync(int id, UpdateDocumentModel document)
        {
            var DocUpdate = await _unitOfWork.DocumentRepository.GetByIdAsync(id);
            if (DocUpdate != null)
            {
                DocUpdate = _mapper.Map(document, DocUpdate);
                await _unitOfWork.DocumentRepository.Update(DocUpdate);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    return new ResponseModel
                    {
                        Status = true,
                        Message = "Document update successfully!"
                    };
                }
                return new ResponseModel
                {
                    Status = false,
                    Message = "Document update failed!"
                };
            }
            return new ResponseModel
            {
                Status = false,
                Message = "Document not found!"
            };
        }
        public async Task<ResponseModel> DeleteDocumentAsync(int id)
        {
            var deleteDocument = await _unitOfWork.DocumentRepository.GetByIdAsync(id);
            if (deleteDocument != null)
            {
                await _unitOfWork.DocumentRepository.SoftRemove(deleteDocument);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseModel { Status = true, Message = "Document deleted successfully" };
            }
            return new ResponseModel { Status = false, Message = "Document deleted false" };
        }
        public async Task<Pagination<DocumentDetailsModel>> GetDocumentFilterAsync(PaginationParameter paginationParameter, DocumentFilterModel documentFilterModel)
        {
            try
            {
                var documents = await _unitOfWork.DocumentRepository.GetDocumentFilterAsync(paginationParameter, documentFilterModel);
                if (documents != null)
                {
                    var mapperResult = _mapper.Map<List<DocumentDetailsModel>>(documents);
                    return new Pagination<DocumentDetailsModel>(mapperResult, documents.TotalCount, documents.CurrentPage, documents.PageSize);
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
