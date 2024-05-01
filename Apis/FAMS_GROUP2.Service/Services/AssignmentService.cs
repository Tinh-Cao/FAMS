using Application.ViewModels.ResponseModels;
using AutoMapper;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.ViewModels.AssignmentModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Services.Interfaces;

namespace FAMS_GROUP2.Services.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IUnitOfWork _repo;
        private readonly IMapper _mapper;

        public AssignmentService(IUnitOfWork repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<AssignmentResponseModel> CreateAsmByExcelAsync(int moduleId,
            List<AssignmentImportModel> listModel)
        {
            var listName = listModel.Select(entity => entity.AssignmentName?.ToUpper()).ToList();
            
            var asmExistedByName = await _repo.AssignmentRepository.GetAsmsByNameAsync(moduleId, listName);

            var asmIsDuplicatedByName = new List<AssignmentImportModel>();

            if (asmExistedByName.Any())
            {
                asmIsDuplicatedByName = listModel.Join(
                    asmExistedByName, 
                    entity => entity.AssignmentName,
                    name => name,
                    (entity, _) => entity).ToList();
                
                listModel = listModel.Where(entity =>
                        !asmExistedByName.Any(name => name.Equals(entity.AssignmentName)))
                    .ToList();
            }
            
            var mapperObj = listModel.Select(model =>
            {
                var assignment = _mapper.Map<Assignment>(model);
                assignment.ModuleId = moduleId;
                return assignment;
            }).ToList();
            
            await _repo.AssignmentRepository.AddRangeAsyncV2(mapperObj);
            var result = await _repo.SaveChangeAsync();

            if (result < 1)
            {
                return new AssignmentResponseModel
                {
                    Status = false,
                    Message = "Add Failed!",
                    DuplicatedNameAsm = asmIsDuplicatedByName.ToList()
                };
            }

            return new AssignmentResponseModel
            {
                Status = true,
                Message = "Add Successfully!",
                DuplicatedNameAsm = asmIsDuplicatedByName.ToList()
            };
        }

        public async Task<Pagination<AssignmentViewModel>> GetAsmsByFiltersAsync(
            PaginationParameter paginationParameter, AssignmentFilterModel assignmentFilterModel)
        {
            var obj = await _repo.AssignmentRepository.GetAsmsByFiltersAsync(paginationParameter,
                assignmentFilterModel);
            var mappedResult = _mapper.Map<List<AssignmentViewModel>>(obj);
            return new Pagination<AssignmentViewModel>(mappedResult, obj.TotalCount, obj.CurrentPage, obj.PageSize);
        }

        public async Task<ResponseModel> UpdateAsmAsync(int id, AssignmentImportModel model)
        {
            var asmFound = await _repo.AssignmentRepository.GetByIdAsync(id);
            if (asmFound != null)
            {
                asmFound = _mapper.Map(model, asmFound);
                await _repo.AssignmentRepository.Update(asmFound);
                var result = await _repo.SaveChangeAsync();
                if (result > 0)
                {
                    return new ResponseModel
                    {
                        Status = true,
                        Message = "Assignment update successfully!"
                    };
                }

                return new ResponseModel
                {
                    Status = false,
                    Message = "Assignment update failed!"
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Assignment not found!"
            };
        }

        public async Task<AssignmentViewModel?> GetAsmById(int id)
        {
            var asmFound = await _repo.AssignmentRepository.GetByIdAsync(id);
            if (asmFound == null) return null;
            var asmMapper = _mapper.Map<AssignmentViewModel>(asmFound);
            return asmMapper;
        }

        public async Task<ResponseModel> SoftDeleteAsmById(int id)
        {
            var asmFound = await _repo.AssignmentRepository.GetByIdAsync(id);
            
            if (asmFound == null)
                return new ResponseModel
                {
                    Status = false,
                    Message = "Assignment not found!"
                };

            if (!asmFound.Status.Equals("Pending"))
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Assignment is ongoing or is applied!"
                };
            }

            await _repo.AssignmentRepository.SoftRemove(asmFound);
            var result = await _repo.SaveChangeAsync();

            if (result > 0)
            {
                return new ResponseModel
                {
                    Status = true,
                    Message = "Assignment delete successfully!"
                };
            }

            return new ResponseModel
            {
                Status = true,
                Message = "Assignment delete failed!"
            };
        }
    }
}