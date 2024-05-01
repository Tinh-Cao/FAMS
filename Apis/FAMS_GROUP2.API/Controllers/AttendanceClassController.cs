using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.ViewModels.AttendanceModels;
using FAMS_GROUP2.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FAMS_GROUP2.API.Controllers
{
    [Route("api/v1/attendance")]
    [ApiController]
    public class AttendanceClassController : ControllerBase
    {
        private IAttendanceClassService _service;
        public static Semaphore semaphore = new Semaphore(initialCount: 1, maximumCount: 1);
        public AttendanceClassController(IAttendanceClassService service)
        {
            _service = service;
        }
        //get list attendances of class

        [HttpGet("class")]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer,Student")]
        public async Task<IActionResult> GetAttendanceClassByClass([FromQuery] int classId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var tcs = new TaskCompletionSource<AttendanceClassResultModel>();
                await Task.Run(async () =>
                {
                    var result = await _service.GetAttendanceClassByClass(classId, fromDate, toDate);
                    tcs.SetResult(result);
                });
                var output = new
                {
                    tcs.Task.Result.status,
                    tcs.Task.Result.message,
                    list = tcs.Task.Result.listObject?[0]
            
                };
                if (!tcs.Task.Result.status) return BadRequest(output);
                else return Ok(output);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("date")]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer,Student")]

        public async Task<IActionResult> GetAttendanceClassByDate([FromQuery] int classId, DateTime date)
        {

            try
            {
                var tcs = new TaskCompletionSource<AttendanceClassResultModel>();
                await Task.Run(async () =>
                {
                    var result = await _service.GetAttendanceClassByDate(classId, date);
                    tcs.SetResult(result);
                });
                var output = new
                {
                    tcs.Task.Result.status,
                    tcs.Task.Result.message,
                    list = tcs.Task.Result.listObject?[0]
                };
                if (!tcs.Task.Result.status) return BadRequest(output);
                else return Ok(output);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        //get list attendances of student in certain class
        [HttpGet("student")]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer,Student")]

        public async Task<IActionResult> GetAttendanceClassByStudentClass([FromQuery] int classId, [FromQuery] int studentId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var tcs = new TaskCompletionSource<AttendanceClassResultModel>();
                await Task.Run(async () =>
                {
                    var result = await _service.GetAttendanceClassByStudentClass(classId,studentId,fromDate,toDate);
                    tcs.SetResult(result);
                });
                var output = new
                {
                    tcs.Task.Result.status,
                    tcs.Task.Result.message,
                    list = tcs.Task.Result.listObject?[0]
                };
                if (!tcs.Task.Result.status) return BadRequest(output);
                else return Ok(output);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        //add 1 attendance for class
        [HttpPost("class/manually")]
        [Authorize(Roles = "Admin,Trainer")]

        public async Task<IActionResult> AddAttendanceClass([FromBody] List<AttendanceClassViewOfClassModel> request)
        {
            semaphore.WaitOne();
            try
            {
                try
                {
                    var tcs = new TaskCompletionSource<AttendanceClassResultModel>();
                    await Task.Run(async () =>
                    {
                        var result = await _service.AddAttendanceOfClass(request,null, true);
                        tcs.SetResult(result);
                    });
                    var output = new
                    {
                        tcs.Task.Result.status,
                        tcs.Task.Result.message,
                        listClassIdInvalid = tcs.Task.Result.listObject?[0],
                        listAttendanceDuplicate = tcs.Task.Result.listObject?[1],
                        listAttendnaceAdded = tcs.Task.Result.listObject?[2]
                    };
                    return Ok(output);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            finally
            {
                semaphore.Release();
            }
        }
        //add list attendance for class from start date to end date rely on (Monday, Tuesday,.., Sunday)
        //ex: 2024/02/12 to 2024/02/21 (choose Monday , Friday) => list attendances include  2024/02/12, 2024/02/19, 2024/02/19
        [HttpPost("class/range")]
        [Authorize(Roles = "Admin,Trainer")]

        public async Task<IActionResult> AddListAttendanceOfClassByDate([FromBody] AttendanceClassToAddListModel request)
        {
            semaphore.WaitOne();
            try
            {
                try
                {
                    var tcs = new TaskCompletionSource<AttendanceClassResultModel>();
                    await Task.Run(async () =>
                    {
                        var result = await _service.AddListAttendanceOfClassByDate(request);
                        tcs.SetResult(result);
                    });
                    var output = new
                    {
                        tcs.Task.Result.status,
                        tcs.Task.Result.message,
                        listDateDuplicate = tcs.Task.Result.listObject?[0],
                    };
                    if (!tcs.Task.Result.status) return BadRequest(output);
                    else return Ok(output);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            finally
            {
                semaphore.Release();
            }
        }
        //change date of class's certain attendance 
        [HttpPut("class/date")]
        [Authorize(Roles = "Admin,Trainer")]

        public async Task<IActionResult> UpdateDateAttendanceClass([FromBody] AttendanceClassDateUpdateModel request)
        {
            semaphore.WaitOne();
            try
            {
                try
                {
                    var tcs = new TaskCompletionSource<AttendanceClassResultModel>();
                    await Task.Run(async () =>
                    {
                        var result = await _service.UpdateAttendanceClass(request);
                        tcs.SetResult(result);
                    });
                    var output = new
                    {
                        tcs.Task.Result.status,
                        tcs.Task.Result.message
                    };
                    if (!tcs.Task.Result.status) return BadRequest(output);
                    else return Ok(output);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            finally
            {
                semaphore.Release();
            }

        }
        //change status or comment of student's certain attendance 
        [HttpPut("students/detail")]
        [Authorize(Roles = "Admin,Trainer")]

        public async Task<IActionResult> UpdateAttendanceClassDetail([FromBody] AttendanceClassDetailUpdateModel request)
        {
            semaphore.WaitOne();
            try
            {
                try
                {
                    var tcs = new TaskCompletionSource<AttendanceClassResultModel>();
                    await Task.Run(async () =>
                    {
                        var result = await _service.UpdateAttendanceClassDetail(request);
                        tcs.SetResult(result);
                    });
                    var output = new
                    {
                        tcs.Task.Result.status,
                        tcs.Task.Result.message,
                        listStudentIdNotExist = tcs.Task.Result.listObject?[0],
                        listStudentIdDuplicate = tcs.Task.Result.listObject?[1],
                        listStudnetIdLack = tcs.Task.Result.listObject?[2]
                    };
                    if (!tcs.Task.Result.status) return BadRequest(output);
                    else return Ok(output);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            finally
            {
                semaphore.Release();
            }
        }
        //delete class's certain attendance
        [HttpDelete("class")]
        [Authorize(Roles = "Admin,Trainer")]

        public async Task<IActionResult> DeleteAttendanceClassOfClass([FromBody] AttendanceClassViewOfClassModel request)
        {
            semaphore.WaitOne();
            try
            {
                try
                {
                    var tcs = new TaskCompletionSource<AttendanceClassResultModel>();
                    await Task.Run(async () =>
                    {
                        var result = await _service.DeleteAttendanceOfClass(request);
                        tcs.SetResult(result);
                    });
                    var output = new
                    {
                        tcs.Task.Result.status,
                        tcs.Task.Result.message
                    };
                    if (!tcs.Task.Result.status) return BadRequest(output);
                    else return Ok(output);

                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            finally
            {
                semaphore.Release();
            }
        }
        //display total present number, absent number, blank number, present percent, absent percent, blank percent of class's certain attendance
        [HttpGet("class/statistic")]
        [Authorize(Roles = "Admin,Trainer")]

        public async Task<IActionResult> ViewPresentAndAbsentOfClassInDate([FromQuery] int classId, DateTime date)
        {
            try
            {
                var tcs = new TaskCompletionSource<AttendanceClassResultModel>();
                await Task.Run(async () =>
                {
                    var result = await _service.ViewPresentAndAbsentOfClassInDate(classId, date);
                    tcs.SetResult(result);
                });
                var output = new
                {
                    tcs.Task.Result.status,
                    tcs.Task.Result.message,
                    
                    value = (tcs.Task.Result.listObject == null) ? null : new
                    {
                        presentCount = tcs.Task.Result.listObject[0],
                        absentCount = tcs.Task.Result.listObject[1],
                        presentPercent = tcs.Task.Result.listObject[2],
                        absentPercent = tcs.Task.Result.listObject[3]
                    }
                };
                if (!tcs.Task.Result.status) return BadRequest(output);
                else return Ok(output);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        //display total present number, absent number, blank number, present percent, absent percent, blank percent attendance of student in certain class
        [HttpGet("student/statistic")]
        [Authorize(Roles = "Admin,Trainer")]

        public async Task<IActionResult> ViewPresentAndAbsentOfStudentInClass([FromQuery] int classId, [FromQuery] int studentId)
        {
            try
            {
                var tcs = new TaskCompletionSource<AttendanceClassResultModel>();
                await Task.Run(async () =>
                {
                    var result = await _service.ViewPresentAndAbsentOfStudentInClass(classId,studentId);
                    tcs.SetResult(result);
                });
                var output = new
                {
                    tcs.Task.Result.status,
                    tcs.Task.Result.message,

                    value = (tcs.Task.Result.listObject == null) ? null : new
                    {
                        presentCount = tcs.Task.Result.listObject[0],
                        absentCount = tcs.Task.Result.listObject[1],
                        presentPercent = tcs.Task.Result.listObject[2],
                        absentPercent = tcs.Task.Result.listObject[3],
                    }
                };
                if (!tcs.Task.Result.status) return BadRequest(output);
                else return Ok(output);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
