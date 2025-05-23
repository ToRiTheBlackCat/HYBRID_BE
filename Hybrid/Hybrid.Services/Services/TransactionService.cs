using Hybrid.Repositories.Base;
using Hybrid.Repositories.Models;
using Hybrid.Repositories.Repos;
using Hybrid.Services.Constant;
using Hybrid.Services.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.Services
{
    public interface ITransactionService
    {
        Task<(bool, string, string)> CreateTransactionHistory(TransactionRequest request);
        Task<(bool, string)> CancelTransactionHistory(string transactionHistoryId);

        Task<(bool, string, string)> ConfirmTransactionHistory(string transactionHistoryId);
    }

    public class TransactionService : ITransactionService
    {
        private TransactionRepository _transactionRepo => _unitOfWork.TransactionRepo;
        private readonly UnitOfWork _unitOfWork;

        public TransactionService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// FUNC_CreateTransactionHistory
        /// TransactionRequest_ViewModel
        /// (bool, string, string)
        /// Created By: TriNHM
        /// Created Date: 22/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<(bool, string, string)> CreateTransactionHistory(TransactionRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var newTransactionHistory = new TransactionHistory
                {
                    Amount = request.Amount,
                    MethodId = request.MethodId,
                    Status = (int)ConstantEnum.TransactionStatus.PROCCESSING,
                };
                await _unitOfWork.TransactionRepo.CreateAsync(newTransactionHistory);
                await _unitOfWork.CommitTransactionAsync();

                return (true, newTransactionHistory.TransactionId, "Create transaction history successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine(ex.ToString());
                return (false, "", "Create transaction history fail");
            }
        }

        /// <summary>
        /// FUNC_CancelTransactionHistory
        /// transactionHistoryId_string
        /// (bool, string)
        /// Created By: TriNHM
        /// Created Date: 22/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<(bool, string)> CancelTransactionHistory(string transactionHistoryId)
        {
            var foundTransaction = await GetTransactionHistoryById(transactionHistoryId);
            if (foundTransaction == null)
                return (false, "Cannot find any transaction with that id");

            foundTransaction.Status = (int)ConstantEnum.TransactionStatus.CANCELED;
            await _transactionRepo.UpdateAsync(foundTransaction);

            return (true, "Cancel transaction successfully");
        }

        /// <summary>
        /// FUNC_ConfirmTransactionHistory
        /// transactionHistoryId_string
        /// (bool, string, string)
        /// Created By: TriNHM
        /// Created Date: 22/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<(bool, string, string)> ConfirmTransactionHistory(string transactionHistoryId)
        {
            var foundTransaction = await GetTransactionHistoryById(transactionHistoryId);
            if (foundTransaction == null)
                return (false, "", "Cannot find any transaction with that id");

            foundTransaction.Status = (int)ConstantEnum.TransactionStatus.CONFIRMED;
            await _transactionRepo.UpdateAsync(foundTransaction);

            return (true, foundTransaction.TransactionId, "Confirm transaction successfully");
        }

        /// <summary>
        /// FUNC_GetTransactionHistoryById
        /// transactionHistoryId_string
        /// TransactionHistory?
        /// Created By: TriNHM
        /// Created Date: 22/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        private async Task<TransactionHistory?> GetTransactionHistoryById(string transactionHistoryId)
        {
            var foundTransaction = await _transactionRepo.GetByIdAsync(transactionHistoryId);

            return foundTransaction;
        }
    }
}
