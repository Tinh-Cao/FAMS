using Application.ViewModels.ResponseModels;
using AutoMapper;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.Repositories;
using FAMS_GROUP2.Repositories.ViewModels.AccountModels;
using FAMS_GROUP2.Repositories.ViewModels.ModuleModels;
using FAMS_GROUP2.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Services.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ModuleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseModel> CreateModuleAsync(CreateModuleViewModel module)
        {
            var moduleObj = _mapper.Map<Module>(module);
            var addedmodule = await _unitOfWork.ModuleRepository.AddAsync(moduleObj);
            await _unitOfWork.SaveChangeAsync();
            if (addedmodule != null)
            {
                return new ResponseModel { Status = true, Message = "Add Module Successfully!!" };
            }

            return new ResponseModel { Status = false, Message = "Error adding Module" };

        }

        public async Task<ResponseModel> DeleteModuleAsync(int id)
        {
            bool isModuleUsed = await _unitOfWork.ModuleRepository.isModuleUsed(id);
            if (isModuleUsed)
            {
                return new ResponseModel { Status = false, Message = "There is a class that is using module" };
            }
            else
            {
                var deleModule = await _unitOfWork.ModuleRepository.GetByIdAsync(id);
                if (deleModule != null)
                {
                    await _unitOfWork.ModuleRepository.SoftRemove(deleModule);
                    await _unitOfWork.SaveChangeAsync();

                    return new ResponseModel { Status = true, Message = "Module deleted successfully" };
                }
                return new ResponseModel { Status = false, Message = "Module deleted false" };

            }

        }

        public async Task<List<ModuleDetailsModel>> GetAllModuleAsync()
        {
            var modules = await _unitOfWork.ModuleRepository.GetAllAsync();
            if (modules != null)
            {
                var activeModules = modules.Where(m => m.Status == "Active" && m.IsDelete != true).ToList();
                var modulesDetail = _mapper.Map<List<ModuleDetailsModel>>(activeModules).ToList();

                return modulesDetail;
            }
            return null;

        }

        public async Task<ModuleDetailsModel> GetModuleByIDAsync(int moduleId)
        {
            var module = await _unitOfWork.ModuleRepository.GetByIdAsync(moduleId);
            if (module != null)
            {
                var modulesDetail = _mapper.Map<ModuleDetailsModel>(module);
                return modulesDetail;

            }
            return null;
        }

        public async Task<Pagination<ModuleDetailsModel>> GetPaginationAsync(PaginationParameter paginationParameter, ModuleFilterModule moduleFilterModule)
        {
            var modulesQuery = await _unitOfWork.ModuleRepository.GetAllModuleAsyncPaging(paginationParameter, moduleFilterModule);
            var count = modulesQuery.Count();
            var mapmodules = _mapper.Map<List<ModuleDetailsModel>>(modulesQuery).ToList();
            var paginationResult = new Pagination<ModuleDetailsModel>(mapmodules, count, paginationParameter.PageIndex, paginationParameter.PageSize);

            return paginationResult;
        }

        public async Task<ResponseModel> PauseModuleAsync(int id)
        {
            bool isModuleUsed = await _unitOfWork.ModuleRepository.isModuleUsed(id);
            if (isModuleUsed)
            {
                return new ResponseModel { Status = false, Message = "There is a class that is using module" };
            }
            var result = await _unitOfWork.ModuleRepository.GetByIdAsync(id);
            if (result?.IsDelete == true)
            {
                return new ResponseModel { Status = false, Message = "The module has been deleted and cannot be paused" };
            }
            if (result != null)
            {
                result.Status = ModuleStatus.Stop.ToString();
                await _unitOfWork.ModuleRepository.Update(result);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseModel { Status = true, Message = "Module is paused" };
            }
            return new ResponseModel { Status = false, Message = "Module does not exist" };
        }

        public async Task<ResponseModel> UpdateModuleAsync(int id, UpdateModuleViewModel moduleModel)
        {
            var module = await _unitOfWork.ModuleRepository.GetByIdAsync(id);
            if (module == null)
            {
                return new ResponseModel { Status = false, Message = "Module does not exist" };
            }
            if (module.IsDelete == true)
            {
                return new ResponseModel { Status = false, Message = "The module has been deleted and cannot be updated" };
            }
            module.ModuleCode = moduleModel.ModuleCode;
            module.ModuleName = moduleModel.ModuleName;
            module.Status = moduleModel.Status;
            await _unitOfWork.ModuleRepository.Update(module);
            await _unitOfWork.SaveChangeAsync();
            return new ResponseModel { Status = true, Message = "Module updated successfully" };
        }


    }
}

