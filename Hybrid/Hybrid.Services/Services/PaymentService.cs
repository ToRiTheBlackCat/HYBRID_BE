using Hybrid.Services.ViewModel.Payment;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Net.payOS;
using Net.payOS.Types;

namespace Hybrid.Services.Services
{
    public interface IPaymentService
    {
        Task<(long, string)> CreatePaymentRequest(CreatePaymentRequest request, PayOsClient client);
        Task<(long, string)> GetPaymentResponse(long id, PayOsClient client);

    }
    public class PaymentService : IPaymentService
    {
        public PaymentService()
        {

        }
        public async Task<(long, string)> CreatePaymentRequest(CreatePaymentRequest request, PayOsClient client)
        {
            PayOS payOS = new PayOS(client.ClientId, client.ApiKey, client.ChecksumKey);
            var payOsRequest = new CreatePayOsRequest
            {
                OrderCode = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Amount = request.Amount,
                Description = $"Thanh toán đơn hàng {request.TransactionId}",
                BuyerName = request.BuyerName,
                ExpiredAt = (int)DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds()
            };

            PaymentData convertedPaymentData = new PaymentData(
                 orderCode: payOsRequest.OrderCode,
                 amount: payOsRequest.Amount,
                 description: payOsRequest.Description,
                 items: null,
                 cancelUrl: payOsRequest.CancelUrl,
                 returnUrl: payOsRequest.ReturnUrl,
                 buyerName: payOsRequest.BuyerName,
                 expiredAt: payOsRequest.ExpiredAt
             );
            CreatePaymentResult response = await payOS.createPaymentLink(convertedPaymentData);

            return (response.orderCode, response.checkoutUrl);
        }

        public async Task<(long,string)> GetPaymentResponse(long id, PayOsClient client)
        {
            PayOS payOS = new PayOS(client.ClientId, client.ApiKey, client.ChecksumKey);
            var response = await payOS.getPaymentLinkInformation(id);

            return (response.orderCode, response.status);
        }
    }
}
