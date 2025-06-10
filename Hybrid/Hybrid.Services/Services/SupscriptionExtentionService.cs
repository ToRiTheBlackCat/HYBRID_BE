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
    }
    public class SupscriptionExtentionService : ISupscriptionExtentionService
    {

        private  SupscriptionExtentionRepository _supscriptionExtentionRepo => _unitOfWork.SupscriptionExtentionRepo;
        private readonly UnitOfWork _unitOfWork;

        public SupscriptionExtentionService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// FUNC_CreateSupscriptionExtentionOrder_Student
        /// SupscriptionExtentionOrder
        /// (bool, string)
        /// Created By: TriNHM
        /// Created Date: 22/5/2025
        /// Updated By: X
        /// Updated Date: X
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
        /// Updated By: X
        /// Updated Date: X
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
