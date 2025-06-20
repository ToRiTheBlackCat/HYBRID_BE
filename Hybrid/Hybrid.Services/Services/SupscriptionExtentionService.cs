using Hybrid.Repositories.Base;
using Hybrid.Repositories.Models;
using Hybrid.Repositories.Repos;
using Hybrid.Services.Helpers;
using Hybrid.Services.ViewModel.SupscriptionExtention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.Services
{
    public interface ISupscriptionExtentionService
    {
        Task<(bool, string)> CreateSupscriptionExtentionOrder_Student(SupscriptionExtentionOrderRequest request);
        Task<(bool, string)> CreateSupscriptionExtentionOrder_Teacher(SupscriptionExtentionOrderRequest request);
        Task<(bool, string)> CheckExpireSupscriptionExtention(string userId, bool isTeacher);
    }
    public class SupscriptionExtentionService : ISupscriptionExtentionService
    {
        private readonly UnitOfWork _unitOfWork;

        public SupscriptionExtentionService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// FUNC_CheckExpireSupscriptionExtention
        /// userId_string / isTeacher_bool
        /// (bool, string)
        /// Created By: TriNHM
        /// Created Date: 20/6/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<(bool, string)> CheckExpireSupscriptionExtention(string userId, bool isTeacher)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (isTeacher)
                {
                    var latest_teacherSupscription = await _unitOfWork.TeacherSupscriptionRepo.GetLatestSupscriptionOfTeacher(userId);

                    if (latest_teacherSupscription != null && DateTime.Now > latest_teacherSupscription.EndDate)
                    {
                        var isUpdated = await UpdateTier(userId, true);
                        if (!isUpdated)
                        {
                            return (false, "Fail to reupdate tier");
                        }

                        latest_teacherSupscription.IsActive = false;
                        _unitOfWork.TeacherSupscriptionRepo.Update(latest_teacherSupscription); 

                        await _unitOfWork.SaveChangesAsync();
                    }
                    else
                    {
                        return (false, "Not exist any supscription extension or supscription extension not expired yet");
                    }
                }
                else
                {
                    var latest_studentSupscription = await _unitOfWork.StudentSupscriptionRepo.GetLatestSupscriptionOfStudent(userId);

                    if (latest_studentSupscription != null && DateTime.Now > latest_studentSupscription.EndDate)
                    {
                        var isUpdated = await UpdateTier(userId, false);
                        if (!isUpdated)
                        {
                            return (false, "Fail to reupdate tier");
                        }

                        latest_studentSupscription.IsActive = false;
                        _unitOfWork.StudentSupscriptionRepo.Update(latest_studentSupscription);

                        await _unitOfWork.SaveChangesAsync();
                    }
                    else
                    {
                        return (false, "Not exist any supscription extension or supscription extension not expired yet");
                    }
                }
                
                await _unitOfWork.CommitTransactionAsync();
                return (true, "Check expirity and reupdate tier successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.Write(ex.Message);
                return (false, "Fail to get the supscription extension and reupdate the tier");
            }
        }

        /// <summary>
        /// FUNC_UpdateTier
        /// userId_string / isTeacher_bool
        /// (bool)
        /// Created By: TriNHM
        /// Created Date: 20/6/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        private async Task<bool> UpdateTier(string userId, bool isTeacher)
        {
            bool isSuccess = true;
            try
            {
                if (isTeacher)
                {
                    var foundTeacher = await _unitOfWork.TeacherRepo.GetByIdAsync(userId);
                    if (foundTeacher != null)
                    {
                        foundTeacher.TierId = "1";
                        _unitOfWork.TeacherRepo.Update(foundTeacher);
                    }
                }
                else
                {
                    var foundStudent = await _unitOfWork.StudentRepo.GetByIdAsync(userId);
                    if (foundStudent != null)
                    {
                        foundStudent.TierId = "1";
                        _unitOfWork.StudentRepo.Update(foundStudent);
                    }
                }
                await _unitOfWork.SaveChangesAsync();
                return isSuccess;
            }
            catch (Exception ex)
            {
                return !isSuccess;
            }
        }

        /// <summary>
        /// FUNC_CreateSupscriptionExtentionOrder_Student
        /// SupscriptionExtentionOrder
        /// (bool, string)
        /// Created By: TriNHM
        /// Created Date: 22/5/2025
        /// Updated By: TriNHM
        /// Updated Date: 20/6/2025
        /// </summary>
        public async Task<(bool, string)> CreateSupscriptionExtentionOrder_Student(SupscriptionExtentionOrderRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var newSupscriptionOrder = request.Map_SupscriptionExtentionOrderRequest_To_SupscriptionExtentionOrder();
                await _unitOfWork.SupscriptionExtentionRepo.CreateAsync(newSupscriptionOrder);

                var newStudentSupscription = newSupscriptionOrder.Map_SupscriptionExtentionOrder_To_StudentSupscription();

                await _unitOfWork.StudentSupscriptionRepo.CreateAsync(newStudentSupscription);

                await _unitOfWork.CommitTransactionAsync();
                return (true, "Create Supscription extention order successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.Write(ex.Message);
                return (false, "Create Supscription extention order fail");
            }
        }

        /// <summary>
        /// FUNC_CreateSupscriptionExtentionOrder_Teacher
        /// SupscriptionExtentionOrder
        /// (bool, string)
        /// Created By: TriNHM
        /// Created Date: 22/5/2025
        /// Updated By: TriNHM
        /// Updated Date: 20/6/2025
        /// </summary>
        public async Task<(bool, string)> CreateSupscriptionExtentionOrder_Teacher(SupscriptionExtentionOrderRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var newSupscriptionOrder = request.Map_SupscriptionExtentionOrderRequest_To_SupscriptionExtentionOrder();
                await _unitOfWork.SupscriptionExtentionRepo.CreateAsync(newSupscriptionOrder);

                var newTeacherSupscription = newSupscriptionOrder.Map_SupscriptionExtentionOrder_To_TeacherSupscription();

                await _unitOfWork.TeacherSupscriptionRepo.CreateAsync(newTeacherSupscription);

                await _unitOfWork.CommitTransactionAsync();
                return (true, "Create Supscription extention order successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.Write(ex.Message);
                return (false, "Create Supscription extention order fail");
            }
        }
    }
}
