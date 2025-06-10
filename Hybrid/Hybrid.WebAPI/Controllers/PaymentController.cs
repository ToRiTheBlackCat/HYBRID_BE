using Hybrid.Services.Helpers;
using Hybrid.Services.Services;
using Hybrid.Services.ViewModel.Payment;
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
        public async Task<ActionResult> CreatePaymentRequest([FromBody] CreatePaymentRequest request)
        {
            PayOsClient client = new PayOsClient
            {
                ClientId = HybridVariables.PayOsClientId,
                ApiKey = HybridVariables.PayOsApiKey,
                ChecksumKey = HybridVariables.PayOsCheckSumKey
            };

            var checkoutUrl = await _paymentService.CreatePaymentRequest(request, client);

            return Ok();
        }
    }
}
