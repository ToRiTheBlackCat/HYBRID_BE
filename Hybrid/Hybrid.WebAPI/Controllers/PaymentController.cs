using Hybrid.Services.Helpers;
using Hybrid.Services.Services;
using Hybrid.Services.ViewModel.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hybrid.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// API_CreatePaymentRequest
        /// CreatePaymentRequest_ViewModel
        /// Checkout URL _ string
        /// Created By: TriNHM
        /// Created Date: 10/6/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("payment-requests")]
        [Authorize]
        public async Task<ActionResult> CreatePaymentRequest([FromBody] CreatePaymentRequest request)
        {
            PayOsClient client = new PayOsClient();

            var (orderCode, checkoutUrl) = await _paymentService.CreatePaymentRequest(request, client);

            return Ok(new
            {
                orderCode,
                checkoutUrl,
            });
        }

        /// <summary>
        /// API_CheckPaymentResponse
        /// id_long
        /// orderCode_long / status_string
        /// Created By: TriNHM
        /// Created Date: 17/6/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpGet("check-payment/{id}")]
        [Authorize]
        public async Task<ActionResult> CheckPaymentResponse(long id)
        {
            PayOsClient client = new PayOsClient();
            var (orderCode, status) = await _paymentService.GetPaymentResponse(id, client);

            return Ok(new
            {
                orderCode,
                status,
            });
        }
    }
}

