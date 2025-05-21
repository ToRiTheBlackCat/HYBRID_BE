using Azure.Core;
using Hybrid.Services.Services;
using Hybrid.Services.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hybrid.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// API_CreateTransactionHistory
        /// TransactionRequest_ViewModel
        /// X
        /// Created By: TriNHM
        /// Created Date: 21/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("create-history")]
        public async Task<ActionResult> CreateTransactionHistory([FromBody] TransactionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (isSuccess, transactionId, message) = await _transactionService.CreateTransactionHistory(request);
            return Ok(new
            {
                isSuccess,
                transactionId = transactionId.Trim(),
                message
            });
        }

        /// <summary>
        /// API_ConfirmTransactionHistory
        /// transactionHistoryId_String
        /// X
        /// Created By: TriNHM
        /// Created Date: 21/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("accept-history")]
        public async Task<ActionResult> ConfirmTransactionHistory(string transactionHistoryId)
        {
            if (string.IsNullOrEmpty(transactionHistoryId))
                return BadRequest(ModelState);

            var (isSuccess, transactionId, message) = await _transactionService.ConfirmTransactionHistory(transactionHistoryId);
            return Ok(new
            {
                isSuccess,
                transactionId = transactionId.Trim(),
                message
            });
        }

        /// <summary>
        /// API_CancelTransactionHistory
        /// transactionHistoryId_String
        /// X
        /// Created By: TriNHM
        /// Created Date: 21/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("cancel-history")]
        public async Task<ActionResult> CancelTransactionHistory(string transactionHistoryId)
        {
            if (string.IsNullOrEmpty(transactionHistoryId))
                return BadRequest(ModelState);

            var (isSuccess, message) = await _transactionService.CancelTransactionHistory(transactionHistoryId);

            return Ok(new
            {
                isSuccess,
                message
            });
        }
    }
}
