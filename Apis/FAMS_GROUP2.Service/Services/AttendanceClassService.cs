using AutoMapper;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces;
using FAMS_GROUP2.Repositories.Interfaces.UoW;
using FAMS_GROUP2.Repositories.ViewModels.AttendanceModels;
using FAMS_GROUP2.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Services.Services
{
    public class AttendanceClassService:IAttendanceClassService
    {
        private readonly IStudentClassService _studentClassService;
        private readonly IUnitOfWork _unitOfWork;
        public AttendanceClassService(IStudentClassService studentClassService,IStudentService studentService, IUnitOfWork unitOfWork)
        {
            _studentClassService = studentClassService;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<AttendanceClass>> GetAttendanceClass(int? studentClassId)
        {
            return await _unitOfWork.AttendanceClassRepository.GetAllAttendanceClassAsync(studentClassId);
        }
        public async Task<List<AttendanceClass>> GetAttendanceClassIsDelete(int? studentClassId)
        {
            return await _unitOfWork.AttendanceClassRepository.GetAllAttendanceClassIsDeleteAsync(studentClassId);
        }
        public async Task<List<AttendanceClassModel>> GetAttendanceClassDTO(int? studentClassId)
        {
           return AttendanceClassModel.ListModelConvert(await GetAttendanceClass(studentClassId)); 
        }

        public async Task<AttendanceClassResultModel> GetAttendanceClassByClass(int classID, DateTime fromDate, DateTime toDate)
        {
            List<object?>? listResult = new();
            if (toDate < fromDate && toDate != new DateTime(0001, 01, 01))
            {
                listResult.Add(null);
                return AttendanceClassResultModel.ReturnResult(false, $"Invalid Date!", listResult);
            }
            DateTime toDateNew = toDate.Date;
            if (toDateNew.Date == new DateTime(0001, 01, 01)) toDateNew = new DateTime(9999, 12, 31);
            var studentClass = (await _studentClassService.GetStudentClass()).ToList().SingleOrDefault(l => l.ClassId == classID && l.StudentId == null);
            if (studentClass == null)
            {
                listResult.Add(null);
                return AttendanceClassResultModel.ReturnResult(false, $"classId {classID} is not Exist!", listResult);
            }
            var listAttendanceClass = (await GetAttendanceClassDTO(studentClass.Id)).FindAll(l => l.Date >= fromDate.Date && l.Date <= toDateNew.Date);
            
            listResult.Add(listAttendanceClass);
            return AttendanceClassResultModel.ReturnResult(true, $"successfully", listResult);
        }
        public async Task<AttendanceClassResultModel> GetAttendanceClassByDate(int classId, DateTime date)
        {
            List<object?>? listResult = new();
            var listStudentClass = (await _studentClassService.GetStudentClass()).ToList().FindAll(l => l.ClassId == classId);
            if (listStudentClass.Count == 0)
            {
                return AttendanceClassResultModel.ReturnResult(false, $"classId {classId} is not Exist!", null);
            }
            if ((await GetAttendanceClass(listStudentClass.SingleOrDefault(l => l.StudentId == null)?.Id)).SingleOrDefault(l => l.Date == date) == null)
            {
                return AttendanceClassResultModel.ReturnResult(false, $"date {date.Date.ToString("dd/MM/yyyy")} is not Exist in Class!", null);
            }
            listStudentClass.RemoveAt(0);
            var listStudentOrigin = await _unitOfWork.StudentRepository.GetAllAsync();
            List<AttendanceClassDetailModel> listAttendanceClass = new();

            foreach (StudentClass item in listStudentClass)
            {
                var student = listStudentOrigin.SingleOrDefault(l => l.Id == item.StudentId);
                var attendanceClass = (await GetAttendanceClass(item.Id)).SingleOrDefault(l => l.Date == date.Date);
                if (attendanceClass == null || student == null) continue;
                listAttendanceClass.Add(new AttendanceClassDetailModel()
                {
                    studentId = item.StudentId,
                    studentCode = student.StudentCode,
                    fullName = student.FullName,
                    status = attendanceClass.Status,
                    comment = attendanceClass.Comment

                });
            }
            listResult.Add(listAttendanceClass);
            return AttendanceClassResultModel.ReturnResult(true, "successfully", listResult);
        }
        public async Task<AttendanceClassResultModel> GetAttendanceClassByStudentClass(int classId, int studentId, DateTime fromDate, DateTime toDate)
        {
            List<object?>? listResult = new();
            if (toDate < fromDate && toDate != new DateTime(0001, 01, 01))
            {
                return AttendanceClassResultModel.ReturnResult(false, "Invalid date!", null);
            }
            DateTime toDateNew = toDate;
            if (toDateNew.Date == new DateTime(0001, 01, 01)) toDateNew = new DateTime(9999, 12, 31);
            var listStudentClass = (await _studentClassService.GetStudentClass()).ToList().FindAll(l => l.ClassId == classId);
            if (listStudentClass.Count == 0)
            {
                return AttendanceClassResultModel.ReturnResult(false, $"classId {classId} is not Exist!", null);
            }
            var studentClass = listStudentClass.SingleOrDefault(l => l.StudentId == studentId);
            if (studentClass == null)
            {
                return AttendanceClassResultModel.ReturnResult(false, $"studentId {studentId} is not Exist in Class!", null);
            }
            var listAttendanceClass = GetAttendanceClassDTO(studentClass.Id).Result.ToList().FindAll(l => l.Date >= fromDate.Date && l.Date <= toDateNew.Date);
            
            listResult.Add(listAttendanceClass);
            return AttendanceClassResultModel.ReturnResult(true, $"successfully", listResult);
        }

        public async Task<AttendanceClass?> AttendanceAddOrUpdateIfIsDeleted(AttendanceClass? itemIsDeleted,int? itemId, DateTime? itemDate)
        {
            try
            {
                //kiểm tra xem attendance bị xóa có tồn tại hay không
                if (itemIsDeleted != null)
                {
                    //nếu có thì thay đổi trạng thái
                    itemIsDeleted.Status = null;
                    itemIsDeleted.Comment = null;
                    itemIsDeleted.IsDelete = false;
                    await _unitOfWork.AttendanceClassRepository.UpdateAttendanceClassAsync(itemIsDeleted);
                    return itemIsDeleted;
                }
                else
                {
                    //nếu không thì thêm mới
                    AttendanceClass newAttendanceClass = new AttendanceClass()
                    {
                        StudentClassId = itemId,
                        Date = itemDate,
                        Status = null,
                        Comment = null,
                    };
                    await _unitOfWork.AttendanceClassRepository.AddAttendanceClassAsync(newAttendanceClass);
                    return newAttendanceClass;
                }
            }
            catch(Exception)
            {
                throw;
            }
        }
        public async Task<AttendanceClassResultModel> AddAttendanceOfClass(List<AttendanceClassViewOfClassModel> request,List<AttendanceClass>? listAttendanceTransmission , bool isSaveChanges)
        {
            //khởi tạo các giá trị cần thiết
            List<object?>? listResult = new();
            List<int> listClassIdInvalid = new();
            List<AttendanceClassModel> listAttendanceDuplicate = new();
            List<AttendanceClassModel> listAttendanceResult = new();
            
            //lấy ra danh sách các giá trị không bị trùng lập
            request = request.DistinctBy(l=> new {l.ClassId,l.Date.Date}).ToList();
            
            //chạy từng giá trị trong danh sách
            foreach (var itemMain in request)
            {
                //lấy ra toàn bộ danh sách học sinh trong class bao gồm gốc
                var listStudentClass = 
                    (await _studentClassService.GetStudentClass()).ToList().FindAll(l => l.ClassId == itemMain.ClassId);

                //kiểm tra xem classId có hợp lệ không
                //nếu không thì thêm vào danh sách classId không hợp lệ
                if (listStudentClass.Count == 0)
                {
                    listClassIdInvalid.Add(itemMain.ClassId);
                    continue;
                }

                AttendanceClass? attendanceClass = new();
                //kiểm tra xem có sẵn danh sách attendance gốc chưa
                if (listAttendanceTransmission == null) 
                    //lấy ra attendance gốc từ database
                    attendanceClass = 
                        (await GetAttendanceClass(listStudentClass.SingleOrDefault(l => l.StudentId == null)?.Id))
                        .ToList().SingleOrDefault(l => l.Date == itemMain.Date.Date);
                //lấy ra attendance gốc từ danh sách có sẵn
                else attendanceClass = listAttendanceTransmission.SingleOrDefault(l => l.Date == itemMain.Date.Date);

                //kiểm tra attendance gốc này có tồn tại trong class chưa
                //nếu đã có rồi thì thêm vào danh sách duplicate
                if (attendanceClass != null)
                {
                    listAttendanceDuplicate.Add(AttendanceClassModel.ObjectModelConvert(attendanceClass));
                    continue;
                }

                AttendanceClass? attendanceClassForAdd = null;
                //lấy ra danh sách attendance đã bị xóa
                var listAttendanceClassIsDelete = await GetAttendanceClassIsDelete(null);
                foreach (var item in listStudentClass)
                {
                    //lấy ra 1 attendance cụ thể từ danh sách attendance đã bị xóa
                    var attendanceClassIsDelete =
                        listAttendanceClassIsDelete.
                        SingleOrDefault(l => l.StudentClassId == item?.Id && l.Date == itemMain.Date.Date);

                    //thực hiện hàm AttendanceAddOrUpdateIfIsDeleted
                    //và chỉ gán 1 giá trị duy nhất vào attendanceClassForAdd
                    if (attendanceClassForAdd == null)
                    {
                        attendanceClassForAdd =
                        (await AttendanceAddOrUpdateIfIsDeleted(attendanceClassIsDelete, item?.Id, itemMain.Date.Date));
                    }
                    else
                    {
                        await AttendanceAddOrUpdateIfIsDeleted(attendanceClassIsDelete, item?.Id, itemMain.Date.Date);
                    }
                }

                //thêm attendance vừa mới add và danh sách kết quả
                if (attendanceClassForAdd != null) 
                    listAttendanceResult.Add(AttendanceClassModel.ObjectModelConvert(attendanceClassForAdd));
            }

            //thực hiện lưu database nếu được phép
            if (isSaveChanges)
                await _unitOfWork.AttendanceClassRepository.SaveChangesAsync();

            //xuất kết quả
            listResult.Add(listClassIdInvalid);
            listResult.Add(listAttendanceDuplicate);
            listResult.Add(listAttendanceResult);
            return AttendanceClassResultModel.ReturnResult(true, $"successfully", listResult);
        }

        public async Task<AttendanceClassResultModel> AddListAttendanceOfClassByDate(AttendanceClassToAddListModel request)
        {
            List<object?>? listResult = new();

            //kiểm tra tính hợp lệ đầu vào của ngày
            if (request.FromDate.Date > request.ToDate.Date)
            {
                return AttendanceClassResultModel.ReturnResult(false, "Invalid date!", null);
            }

            //lấy danh sách attendance gốc theo classId
            var studentClassMain = 
                (await _studentClassService.GetStudentClass())
                .ToList().SingleOrDefault(l => l.ClassId == request.ClassId && l.StudentId == null);
            List<AttendanceClass> listAttendanceClassMain = (await GetAttendanceClass(studentClassMain?.Id));

            DateTime dateTime = request.FromDate.Date;
            List<String> listDateDuplicate = new();
            List<DateTime>? listDateExclusion = request.ListExclusionDate;

            //loại bỏ những ngày không hợp lệ trong danh sách những ngày ngoại lệ
            //những ngày nằm ngoài thời lượng thêm vào
            listDateExclusion?.RemoveAll(l => l.Date < request.FromDate.Date || l.Date > request.ToDate.Date);

            //kiểm trà từng ngày trong thời lượng thêm vào
            while (dateTime <= request.ToDate.Date)
            {
                //kiểm tra xem ngày đó có phải ngày cần thêm và không thuộc ngoại lệ không 
                if (CheckDayOfWeek(dateTime, request.DayOfWeek) == true && CheckExclusionDate(dateTime, listDateExclusion) == false)
                {                           
                    //thực hiện hàm add 1 attendance vào lớp đó
                    var validValue = await AddAttendanceOfClass(new List<AttendanceClassViewOfClassModel>
                    {
                        new AttendanceClassViewOfClassModel
                        {
                        ClassId = request.ClassId,
                        Date = dateTime
                        }
                    }, listAttendanceClassMain, false);

                    //nếu hàm trả về classId không hợp lệ thì return
                    if (((List<int>?)validValue.listObject?[0])?.Count > 0)
                    {
                        return AttendanceClassResultModel.ReturnResult(false, $"classId {request.ClassId} is not Exist!", null);
                    }
                    //nếu hàm trả về ngày này đã tồn tại thì thêm ngày đó và danh sách duplicate
                    else if (((List<AttendanceClassModel>?)validValue.listObject?[1])?.Count > 0) 
                        listDateDuplicate.Add(dateTime.Date.ToString("dd/MM/yyyy"));
                }
                //thực hiện hàm chuyển đổi ngày tiếp theo 
                //và gán vào giá trị hiện tại để tiếp tục thực thi vòng lặp while
                dateTime = NextDate(dateTime);
            }
            //lưu thay đổi database khi mọi thứ đã được thêm vào xong
            await _unitOfWork.AttendanceClassRepository.SaveChangesAsync();

            //lưu kết quả danh sách ngày trùng lặp và return
            listResult.Add(listDateDuplicate);
            return AttendanceClassResultModel.ReturnResult(true, "Add successfully", listResult);
        }
        private bool CheckExclusionDate(DateTime currentDate, List<DateTime>? listExclusionDate)
        {
            //kiểm tra currentDate có thuộc danh sách ngày ngoại lệ hay không
            if (listExclusionDate != null && listExclusionDate.Contains(currentDate.Date.Date)) return true;
            return false;
        }
        private bool CheckDateTimeExists(int year, int month , int day)
        {
            //kiểm tra giá trị của month 'tháng' có vượt quá 12 không
            if (month > 12) return false;
            
            //kiểm tra tổng số ngày của year 'năm' và month 'tháng' có vượt quá day 'ngày' hiện tại
            //ex: 31>=20 => true;
            bool isValid = DateTime.DaysInMonth(year, month) >= day;
            return isValid;
        }
        private bool CheckDayOfWeek(DateTime dateTime, DaysOfWeekModel? dayOfWeek)
        {
            //kiểm tra xem dateTime có phải là ngày cho phép hay không
            //nếu thuộc 1 trong các ngày cho phép thì return true
            //nếu không thì return false
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    if (dayOfWeek?.IsSunday == true) return true;
                    else return false;
            
                case DayOfWeek.Monday:
                    if (dayOfWeek?.IsMonday == true) return true;
                    else return false;
               
                case DayOfWeek.Tuesday:
                    if (dayOfWeek?.IsTuesday == true) return true;
                    else return false;
               
                case DayOfWeek.Wednesday:
                    if (dayOfWeek?.IsWednesday == true) return true;
                    else return false;
               
                case DayOfWeek.Thursday:
                    if (dayOfWeek?.IsThursday == true) return true;
                    else return false;
                    
                case DayOfWeek.Friday:
                    if (dayOfWeek?.IsFriday == true) return true;
                    else return false;
                    
                case DayOfWeek.Saturday:
                    if (dayOfWeek?.IsSaturday == true) return true;
                    else return false;
            }
            return false;
        }

        private DateTime NextDate(DateTime currentDate)
        {
            //kiểm tra xem ngày tiếp theo yyyy/MM/DD+1 có phải ngày hợp lệ không
            if (CheckDateTimeExists(currentDate.Year, currentDate.Month, currentDate.Day + 1) == true)
            {
                //trả về yyyy/MM/DD+1
                return new DateTime(currentDate.Year, currentDate.Month, currentDate.Day + 1);
            }
            //kiểm tra xem ngày tiếp theo yyyy/MM+1/1 có phải ngày hợp lệ không
            else if (CheckDateTimeExists(currentDate.Year, currentDate.Month + 1, 1) == true)
            {
                //trả về yyyy/MM+1/1
                return new DateTime(currentDate.Year, currentDate.Month + 1, 1);
            }
            else
            {
                //trả về yyyy+1/1/1
                return new DateTime(currentDate.Year + 1, 1, 1);
            }
        }
        public async Task AddListAttendanceOfNewStudentIntoClass(int classId,int studentId)
        {
            try
            {
                //lấy studentClass dựa trên classId và studentId
                var listStudentClassOrigin = await _studentClassService.GetStudentClass();
                var studentClass = listStudentClassOrigin.ToList().SingleOrDefault(l => l.ClassId == classId && l.StudentId == studentId);
                if (studentClass == null) return;
                //lấy studentClass gốc dựa trên classId
                var studentClassMain = listStudentClassOrigin.ToList().SingleOrDefault(l => l.ClassId == classId && l.StudentId == null);
                //lấy danh sách attendance gốc chưa bị delete và bị delete dựa trên studentClass gốc
                var listAttendanceClassMain = await GetAttendanceClass(studentClassMain?.Id);
                var listAttendanceClassIsDeleteOrigin = await GetAttendanceClassIsDelete(studentClass?.Id);

                //thêm từng attendance từ attendance gốc vào studentClass mới
                foreach(var item in listAttendanceClassMain)
                {
                    var attendanceClassIsDelete = listAttendanceClassIsDeleteOrigin?.SingleOrDefault(l => l.Date == item.Date);
                    await AttendanceAddOrUpdateIfIsDeleted(attendanceClassIsDelete, studentClass?.Id, item.Date);
                }
                await _unitOfWork.AttendanceClassRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AttendanceClassResultModel> UpdateAttendanceClass(AttendanceClassDateUpdateModel request)
        {
            //kiểm tra xem ngày cần update và ngày sẽ được update có trùng nhau không
            if (request.PreviousDate.Date == request.AfterDate.Date)
            {
                return AttendanceClassResultModel.ReturnResult(false, "PreviousDate and AfterDate have to be different!", null);
            }
            //lấy toàn bộ danh sách studentClass bao gồm cả gốc dựa trên classId
            var listStudentClass = (await _studentClassService.GetStudentClass()).ToList().FindAll(l => l.ClassId == request.ClassId);
            if (listStudentClass.Count == 0)
            {
                return AttendanceClassResultModel.ReturnResult(false, $"classId {request.ClassId} does not Exist!", null);
            }
            var listAttendanceClassOrigin = await GetAttendanceClass(listStudentClass.SingleOrDefault(l => l.StudentId == null)?.Id);
            //kiểm tra xem ngày cần update có tồn tài không
            if (listAttendanceClassOrigin.SingleOrDefault(l => l.Date == request.PreviousDate.Date) == null)
            {
                return AttendanceClassResultModel.ReturnResult(false, $"date {request.PreviousDate.Date.ToString("dd/MM/yyyy")} does not Exist in class!", null);
            }
            //kiểm tra xem ngày sẽ được update có tồn tại chưa
            if (listAttendanceClassOrigin.SingleOrDefault(l => l.Date == request.AfterDate.Date) != null)
            {
                return AttendanceClassResultModel.ReturnResult(false, $"date {request.AfterDate.Date.ToString("dd/MM/yyyy")} existed in class!", null);
            }
            List<AttendanceClass> listAttendanceClass = new();
            //lấy ra từng ngày tương ứng trong bảng attendance của mỗi studentCLass
            foreach (var item in listStudentClass)
            {
                var attendanceClass = (await GetAttendanceClass(item.Id)).
                    SingleOrDefault(l => l.Date == request.PreviousDate.Date);
                if (attendanceClass == null) continue;
                listAttendanceClass.Add(attendanceClass);
            }

            //thay đổi cập nhật
            try
            {
                foreach(var item in listAttendanceClass)
                {
                    item.Date = request.AfterDate.Date;
                    await _unitOfWork.AttendanceClassRepository.UpdateAttendanceClassAsync(item);
                }
            }
            finally
            {
                await _unitOfWork.AttendanceClassRepository.SaveChangesAsync();
            }
            return AttendanceClassResultModel.ReturnResult(true, $"Update Attendance successfully!", null);
        }
        public async Task<AttendanceClassResultModel> UpdateAttendanceClassDetail(AttendanceClassDetailUpdateModel request)
        {
            List<object?>? listResult = new();
            //lấy ra danh sách studentClass bao gồm gốc dựa trên classId
            var listStudentClass = (await _studentClassService.GetStudentClass()).
                ToList().FindAll(l => l.ClassId == request.ClassId);
            if (listStudentClass.Count == 0)
            {
                return AttendanceClassResultModel.ReturnResult(false, $"classId {request.ClassId} does not Exist!", null);
            }
            //lấy ra studentClass gốc
            var studentClassMain = listStudentClass.SingleOrDefault(l => l.StudentId == null);
            //lấy ra 1 attendance gốc cụ thể dựa trên date
            var attendanceClassMain = (await GetAttendanceClass(studentClassMain?.Id)).SingleOrDefault(l => l.Date == request.Date.Date);
            if (attendanceClassMain == null)
            {
                return AttendanceClassResultModel.ReturnResult(false, $"date {request.Date.Date.ToString("dd/MM/yyyy")} does not Exist in Class!", null);
            }

            List<int> listStudentIdNonExist = new();
            //kiểm tra và trả về danh sách studentId không tồn tại
            foreach(var item in request.ListStudents)
            {
                var studentClass = listStudentClass.SingleOrDefault(l => l.StudentId == item.StudentId);
                if (studentClass == null)
                {
                    listStudentIdNonExist.Add(item.StudentId);
                }
            }
            if (listStudentIdNonExist.Count > 0)
            {
                listResult.Add(listStudentIdNonExist);
                listResult.Add(null);
                listResult.Add(null);
                return AttendanceClassResultModel.ReturnResult(false, $"List StudentId do not Exist in Class!", listResult);
            }

            //kiểm tra và tra về danh sách studentId bị duplicate
            List<int?> listStudentIdDuplicate = new();
            foreach(var item in request.ListStudents)
            {
                if (request.ListStudents.FindAll(l => l.StudentId == item.StudentId).Count >1 && listStudentIdDuplicate.FirstOrDefault(l=>l == item.StudentId) == null) listStudentIdDuplicate.Add(item.StudentId);
            }
            if (listStudentIdDuplicate.Count > 0)
            {
                listResult.Add(null);
                listResult.Add(listStudentIdDuplicate);
                listResult.Add(null);
                return AttendanceClassResultModel.ReturnResult(false, $"List StudentId is Duplicate!", listResult);
            }

            //kiểm tra và trả về danh sách studentId còn thiếu trong class đó
            var listStudentIdOrigin = (await _studentClassService.GetStudentClass()).ToList().FindAll(l => l.ClassId == request.ClassId && l.StudentId != null);
            if(listStudentIdOrigin.Count != request.ListStudents.Count)
            {
                List<int?> listStudentIdLack = new();
                foreach (var item in listStudentIdOrigin)
                {
                    if (request.ListStudents.SingleOrDefault(l => l.StudentId == item.StudentId) == null) listStudentIdLack.Add(item.StudentId);
                }
                listResult.Add(null);
                listResult.Add(null);
                listResult.Add(listStudentIdLack);
                return AttendanceClassResultModel.ReturnResult(false, $"List StudentId is Lack!", listResult);
            }
            
            //update thông tin chi tiết attendance cụ thể của mỗi student 
            foreach(var item in request.ListStudents)
            {
                var studentClass = listStudentClass.SingleOrDefault(l => l.StudentId == item.StudentId);
                var attendanceClass = (await GetAttendanceClass(studentClass?.Id)).SingleOrDefault(l => l.Date == request.Date.Date);
                if(attendanceClass!= null)
                {
                    attendanceClass.Status = item.Status;
                    attendanceClass.Comment = item.Comment;
                    await _unitOfWork.AttendanceClassRepository.UpdateAttendanceClassAsync(attendanceClass);
                }
            }
            await _unitOfWork.AttendanceClassRepository.SaveChangesAsync();
            return AttendanceClassResultModel.ReturnResult(true, $"Update successfully!", null);

        }

        public async Task<AttendanceClassResultModel> DeleteAttendanceOfClass(AttendanceClassViewOfClassModel request)
        {
            //lấy ra danh sách studentClass bao gồm gốc dựa trên classId
            var listStudentClass = (await _studentClassService.GetStudentClass()).
                ToList().FindAll(l => l.ClassId == request.ClassId);
            if (listStudentClass.Count == 0)
            {
                return AttendanceClassResultModel.ReturnResult(false, $"classId {request.ClassId} does not Exist!", null);
            }
            //kiểm tra attendance cụ thể của list attendance gốc cần delete có tồn tại không dựa vào date
            if ((await GetAttendanceClass(listStudentClass.SingleOrDefault(l => l.StudentId == null)?.Id)).
                SingleOrDefault(l => l.Date == request.Date.Date) == null)
            {
                return AttendanceClassResultModel.ReturnResult(false, $"date {request.Date.Date.ToString("dd/MM/yyyy")} does not Exist in Class!", null);
            }

            //delete từng attendance của mỗi student
            foreach (var item in listStudentClass)
            {
                var attendanceClass = (await GetAttendanceClass(item.Id)).SingleOrDefault(l => l.Date == request.Date.Date);
                if (attendanceClass == null) continue;
                attendanceClass.IsDelete = true;
                await _unitOfWork.AttendanceClassRepository.UpdateAttendanceClassAsync(attendanceClass);

            }
            await _unitOfWork.AttendanceClassRepository.SaveChangesAsync();

            return AttendanceClassResultModel.ReturnResult(true, "Delete successfully", null);
        }
        public async Task DeleteListAttendanceOfStudentOutClass(int classId,int? studentId)
        {
            try
            {
                //lấy ra studentClass dựa trên classId và studentId
                var studentClass = (await _studentClassService.GetStudentClassIsDelete()).
                    ToList().FirstOrDefault(l => l.ClassId == classId && l.StudentId == studentId);
                if (studentClass == null) return;
                var listAttendanceClassStudent = await GetAttendanceClass(studentClass.Id);

                //delete toàn bộ list attendance dựa trên studentClass
                foreach(var item in listAttendanceClassStudent)
                {
                    item.IsDelete = true;
                    await _unitOfWork.AttendanceClassRepository.UpdateAttendanceClassAsync(item);
                }
            }
            catch
            {
                throw;
            }
            
        }
        public async Task<AttendanceClassResultModel> ViewPresentAndAbsentOfClassInDate(int classId, DateTime date)
        {
            List<object?>? listResult = new();
            int presentNumber = 0;
            int absentNumber = 0;
            float presentPercent = 0;
            float absentPercent = 0;
            //lấy ra danh sách studentClass không bao gồm gốc
            var listStudentClass = (await _studentClassService.GetStudentClass()).
                ToList().FindAll(l => l.ClassId == classId && l.StudentId != null);
            if (listStudentClass.Count == 0)
            {
                return AttendanceClassResultModel.ReturnResult(false, $"classId {classId} does not Exist!", null);
            }
            List<AttendanceClass> listAttendanceClassMain = new();
            //lấy ra danh sách attendance dựa trên studentClass và date
            foreach (var item in listStudentClass)
            {
                var attendanceClass = (await GetAttendanceClass(item.Id)).SingleOrDefault(l => l.Date == date.Date);
                if (attendanceClass == null)
                {
                    return AttendanceClassResultModel.ReturnResult(false, $"date {date.Date.ToString("dd/MM/yyyy")} have not Exist in Class!", null);
                }
                listAttendanceClassMain.Add(attendanceClass);
            }
            
            try
            {
                //tăng các giá trị presentNumber và absentNumber dựa trên danh sách vừa mới lấy ra
                Parallel.ForEach(listAttendanceClassMain, item =>
                {
                    if (item == null) return;
                    else if (item?.Status?.ToLower() == "present") presentNumber++;
                    else if (item?.Status?.ToLower() == "absent") absentNumber++;
                });
            }
            finally
            {
                //tính toàn tỉ lệ của presentPercent và absentPercent
                if (listAttendanceClassMain.Count > 0)
                {
                    if(presentNumber == 0 && absentNumber == 0)
                    {
                        presentPercent = 0;
                        absentPercent = 0;
                    }
                    else
                    {
                        presentPercent = (float)presentNumber / (float)(presentNumber + absentNumber);
                        absentPercent = (float)absentNumber / (float)(presentNumber + absentPercent);
                    }
                }
            }

            listResult.Add(presentNumber);
            listResult.Add(absentNumber);
            listResult.Add(presentPercent);
            listResult.Add(absentPercent);
            return AttendanceClassResultModel.ReturnResult(true, $"successfully", listResult);
        }
        public async Task<AttendanceClassResultModel> ViewPresentAndAbsentOfStudentInClass(int classId, int studentId)
        {
            List<object?>? listResult = new();
            int presentNumber = 0;
            int absentNumber = 0;
            float presentPercent = 0;
            float absentPercent = 0;
            //lấy ra toàn bộ list studentClass bao gồm gốc dựa trên classId
            var listStudentClass = (await _studentClassService.GetStudentClass()).
                ToList().FindAll(l => l.ClassId == classId);
            if (listStudentClass.Count == 0)
            {
                return AttendanceClassResultModel.ReturnResult(false, $"classId {classId} does not Exist!", null);
            }
            //lấy ra studentClass dựa trên listStudnetClass và studentId
            var studentClass = listStudentClass.SingleOrDefault(l => l.StudentId == studentId);
            if (studentClass == null)
            {
                return AttendanceClassResultModel.ReturnResult(false, $"studentId {studentId} have not Exist!", null);
            }
            var listAttendanceClass = (await GetAttendanceClass(studentClass.Id));

            try
            {
                //tăng các giá trị presentNumber và absentNumber dựa trên danh sách vừa mới lấy ra
                Parallel.ForEach(listAttendanceClass, item =>
                {
                    if (item == null) return;
                    else if (item?.Status?.ToLower() == "present") presentNumber++;
                    else if (item?.Status?.ToLower() == "absent") absentNumber++;
                });
            }
            finally
            {
                //tính toán tỉ lệ của presentPercent và absentPercent
                if (listAttendanceClass.Count > 0)
                {
                    if (presentNumber == 0 && absentNumber == 0)
                    {
                        presentPercent = 0;
                        absentPercent = 0;
                    }
                    else
                    {
                        presentPercent = (float)presentNumber / (float)(presentNumber + absentNumber);
                        absentPercent = (float)absentNumber / (float)(presentNumber + absentPercent);
                    }
                }
            }

            listResult.Add(presentNumber);
            listResult.Add(absentNumber);
            listResult.Add(presentPercent);
            listResult.Add(absentPercent);
            return AttendanceClassResultModel.ReturnResult(true, $"successfully", listResult);
        }

        public async Task InitialStudentClassOfClass(int classId)
        {
            await _unitOfWork.StudentClassRepository.AddAsync(new StudentClass
            {
                ClassId = classId,
                StudentId = null
            });
            await _unitOfWork.SaveChangeAsync();
        }
        public async Task<bool> InitialStudentClassOfStudent(int classId,int studentId)
        {
            try
            {
                //kiểm tra studentClass dựa trên classId và studentId có tồn tại không
                if ((await _studentClassService.GetStudentClass()).
                    SingleOrDefault(l => l.ClassId == classId && l.StudentId == studentId) != null) return false;
                //lấy ra studentClass bị delete dưa trên classId và studentId và sử lại idDelete nếu có
                var studentClassIsDelete = (await _studentClassService.GetStudentClassIsDelete()).
                    SingleOrDefault(l => l.ClassId == classId && l.StudentId == studentId);
                if (studentClassIsDelete != null)
                {
                    studentClassIsDelete.IsDelete = false;
                    await _unitOfWork.StudentClassRepository.Update(studentClassIsDelete);
                }
                else
                {
                    //thêm studentClass mới dựa trên classId và studentId
                    await _unitOfWork.StudentRepository.UpdateStudentInClass(studentId);
                    StudentClass studentClass = new()
                    {
                        ClassId = classId,
                        StudentId = studentId,
                    };
                    await _unitOfWork.StudentClassRepository.AddAsync(studentClass);
                }
                await _unitOfWork.SaveChangeAsync();
                //thêm toàn bộ danh sách attendance gốc của class đó cho studentClass mơí
                await AddListAttendanceOfNewStudentIntoClass(classId, studentId);
                return true;
            }
            catch
            {
                throw;
            }
                
        }
        public async Task<bool> DeleteStudentClassOfStudent(int classId,int studentId)
        {
            try
            {
                //lấy ra studentClass dựa trên classId và studentId
                StudentClass? studentClass = (await _studentClassService.GetStudentClass()).
                    SingleOrDefault(l => l.ClassId == classId && l.StudentId == studentId);
                if (studentClass == null) return false;
                //update idDelele của studentCLass
                studentClass.IsDelete = true;
                await _unitOfWork.StudentRepository.UpdateStudentOffClass(studentId);
                //delte toàn bộ list attendance của studentClass
                await DeleteListAttendanceOfStudentOutClass(classId, studentId);
                await _unitOfWork.StudentClassRepository.Update(studentClass);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch
            {
                throw;
            }
            
            
        }
        public async Task DeleteAllStudentOfClass(int classId)
        {
            //lấy ra toàn studentClass bao gồm gốc dựa trên classId
            var listStudentClass = ( await _studentClassService.GetStudentClass()).
                ToList().FindAll(l=>l.ClassId == classId);
            foreach(var item in listStudentClass)
            {
                //update isDelte của từng studentClass
                item.IsDelete = true;
                //delte toàn bộ attendance của studentCLass
                await DeleteListAttendanceOfStudentOutClass(classId, item.StudentId);
                await _unitOfWork.StudentClassRepository.Update(item);
            }
            await _unitOfWork.SaveChangeAsync();
        }
    }
}

