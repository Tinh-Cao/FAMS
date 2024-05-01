using AutoMapper;
using FAMS_GROUP2.Repositories;
using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Enums;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.ViewModels.ProgramModels;
using FAMS_GROUP2.Repositories.ViewModels.ResponseModels;
using FAMS_GROUP2.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Identity.Client;
using System.Net.WebSockets;
using System.Reflection;


namespace FAMS_GROUP2.Services.Services
{
    public class ProgramService : IProgramService
    {
       
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitofWork;

        public ProgramService(IMapper mapper, IUnitOfWork unitOfWork)
        {

            _mapper = mapper;
            _unitofWork = unitOfWork;
           
        }
        public async Task<TrainingProgram> CreateProgramAsync(ProgramModel program)
        {
            var programList = await _unitofWork.ProgramRepository.GetAllAsync();
            foreach ( var p in programList.Where(p => p.IsDelete == false))
            {
                if (p.ProgramCode.ToLower() == program.ProgramCode.ToLower()) return null;
            }
            var programObj = _mapper.Map<TrainingProgram>(program);
            if (programObj != null)
            {
                programObj.IsDelete = false;
                programObj.Status = ProgramStatus.Active.ToString();
            }

            //var result = await _programRepository.AddAsync(programObj);
            var result = await _unitofWork.ProgramRepository.AddAsync(programObj);
            await _unitofWork.SaveChangeAsync();


            return result;

        }



        public async Task<List<ProgramResponseModel>> GetAllProgramAsync()
        {
            var a = await _unitofWork.ProgramRepository.GetTrainingPrograms();
            
            
            return a;
           
        }

        public async Task<ProgramResponseModel> GetProgramByIdAsync(int id)
        {
            var program = await _unitofWork.ProgramRepository.GetTrainingProgramById(id);
            
            return _mapper.Map<ProgramResponseModel>(program);
        }

        //public async Task<int> UpdateProgramAsync(int id, UpdateProgramModel updateprogram)
        //{
        //    var pmList = await _unitofWork.ProgramModuleRepository.GetAllAsync();
        //    var mList = await _unitofWork.ModuleRepository.GetAllAsync();
        //    var existedTP = await _unitofWork.ProgramRepository.GetByIdAsync(id);
        //    if (existedTP != null)
        //    {
        //        existedTP.ProgramName = updateprogram.ProgramName;
        //        existedTP.Duration = updateprogram.Duration;
        //        existedTP.Status = updateprogram.Status;
        //        // Check if TP has module                
        //        var hasprogrammodule = pmlist.singleordefault(x => x.programid == id && x.moduleid == updateprogram.updatemodule.id);
        //        if (hasprogrammodule != null)
        //        {
        //            var hasmodule = mlist.singleordefault(m => m.id == updateprogram.updatemodule.id);
        //            if (hasmodule != null)
        //            {
        //                hasmodule.status = updateprogram.updatemodulelist..status;
        //                hasmodule.modulename = updateprogram.updatemodule.modulename;
        //                await _unitofWork.ModuleRepository.Update(hasModule);
        //                await _unitofWork.ProgramRepository.Update(existedTP);
        //                await _unitofWork.SaveChangeAsync();
        //                // return 1 when update tp and module
        //                return 1;
        //            }

        //        }
        //        // return 2 when can update tp and not update module
        //        await _unitofWork.ProgramRepository.Update(existedTP);
        //        await _unitofWork.SaveChangeAsync();
        //        return 2;
        //    }
        //    return 3;
        //}
        public async Task<int> UpdateProgramAsync(int id, UpdateProgramModel updateprogram)
        {
            var pmList = await _unitofWork.ProgramModuleRepository.GetAllAsync();
            var mList = await _unitofWork.ModuleRepository.GetAllAsync();
            var existedTP = await _unitofWork.ProgramRepository.GetByIdAsync(id);
            if (existedTP != null)
            {
                if (updateprogram.ProgramName != null) existedTP.ProgramName = updateprogram.ProgramName;
                if (updateprogram.Duration != null) existedTP.Duration = updateprogram.Duration;
                if (updateprogram.Status != null) existedTP.Status = updateprogram.Status;

                if (updateprogram.ListIdForAdd != null)
                {
                    foreach (var aId in updateprogram.ListIdForAdd)
                    {
                        var existedPM = pmList.FirstOrDefault(x => x.ModuleId == aId && x.ProgramId == id);
                        var existedModule = await _unitofWork.ModuleRepository.GetByIdAsync(aId);

                        if (existedPM == null && existedModule != null)
                        {
                            await _unitofWork.ProgramRepository.AddModuleToProgramAsync(id, aId);
                            await _unitofWork.SaveChangeAsync();
                        }
                        if (existedPM != null && existedPM.IsDelete == true)
                        {
                            //existedPM.IsDelete = existedPM.IsDelete == false ? true : false;
                            existedPM.IsDelete = false;
                            await _unitofWork.ProgramModuleRepository.Update(existedPM);
                            await _unitofWork.SaveChangeAsync();
                        }

                    }
                }
                if (updateprogram.ListIdForRemove != null)
                {
                        foreach (var rId in updateprogram.ListIdForRemove)
                        {
                            var existedPM = pmList.FirstOrDefault(x => x.ModuleId == rId && x.ProgramId == id);
                            if (existedPM != null)
                            {
                                existedPM.IsDelete = true;
                                await _unitofWork.ProgramModuleRepository.Update(existedPM);
                                await _unitofWork.SaveChangeAsync();
                            }
                        }
                }

                    await _unitofWork.ProgramRepository.Update(existedTP);
                    await _unitofWork.SaveChangeAsync();
                    //return 1 when has ModuleIdList
                    return 1;

                    //return 2 when just update tp
                    //await _unitofWork.ProgramRepository.Update(existedTP);
                    //await _unitofWork.SaveChangeAsync();
                    //return 2;

                
                // when can not find tp
                //return 3;
            }
            return 2;
        }
        public async Task<int> UpdateProgramStatusAsync(int id)
        {
            var listClass = await _unitofWork.ClassRepository.GetAllAsync();
            var existedProgram = await _unitofWork.ProgramRepository.GetByIdAsync(id);
            if (existedProgram != null)
            {
                var filterIdClass = listClass.Where(x => x.ProgramId == id).ToList();

                foreach (var x in listClass)
                {
                    if (x.Status.Equals("Ongoing"))
                    {
                        return 1;
                    }
                }
                existedProgram.Status = "Stop";
                await _unitofWork.ProgramRepository.Update(existedProgram);
                await _unitofWork.SaveChangeAsync();
                return 2;
            }
            return 0;
        }
        public async Task<int> DeleteProgramAsync(int id)
        {   var pmList = await _unitofWork.ProgramModuleRepository.GetAllAsync();
            //var mList = await _unitofWork.ModuleRepository.GetAllAsync();
            var existedProgram = await _unitofWork.ProgramRepository.GetByIdAsync(id);
            if (existedProgram != null)
            {
                var listProgramModule = pmList.Where(pm => pm.ProgramId == id).ToList();
                if (listProgramModule != null)
                {
                    foreach (var x in listProgramModule)
                    {
                        x.IsDelete = true;
                        
                    }
                    await _unitofWork.ProgramModuleRepository.UpdateRange(listProgramModule);
                    //await _unitofWork.SaveChangeAsync();
                    existedProgram.IsDelete = true;
                    await _unitofWork.ProgramRepository.Update(existedProgram);
                    await _unitofWork.SaveChangeAsync();
                    return 1;
                }
            }
            return 0;
        }

        public async Task<Pagination<ProgramResponseModel>> GetProgramsByFiltersAsync(PaginationParameter paginationParameter, ProgramFilterModel programFilterModel)
        {
            var programs = await _unitofWork.ProgramRepository.GetTrainingProgramsByFiltersAsync(paginationParameter, programFilterModel);
            if (programs == null)
            {
                return null;
            }
            //var mappedResult1 = _mapper.Map<List<TrainingProgram>>(programs);
            var mappedResult = _mapper.Map<List<ProgramResponseModel>>(programs);
            return new Pagination<ProgramResponseModel>(mappedResult, programs.TotalCount, programs.CurrentPage, programs.PageSize);
        }

        public async Task<bool> AddModuleToProgramAsync(int programId, int moduleId)
        {
            return await _unitofWork.ProgramRepository.AddModuleToProgramAsync(programId, moduleId);
        }

    }

}
