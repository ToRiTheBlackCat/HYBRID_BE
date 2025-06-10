using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Payment
{
    public class CreatePaymentRequest
    {
        public string TransactionId { get; set; }
        public int Amount { get; set; }
        public string BuyerName { get; set; }

    }
}
