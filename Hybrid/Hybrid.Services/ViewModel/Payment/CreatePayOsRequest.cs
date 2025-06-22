using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Payment
{
    public class CreatePayOsRequest
    {
        public long OrderCode { get; set; }
        public int Amount { get; set; }
        public string? Description { get; set; }
        public string? BuyerName { get; set; }
        public string CancelUrl { get; set; } = "https://hybrid-e-learn.netlify.app/processing-payment";
        public string ReturnUrl { get; set; } = "https://hybrid-e-learn.netlify.app/processing-payment";
        public long? ExpiredAt { get; set; }
    }
}
