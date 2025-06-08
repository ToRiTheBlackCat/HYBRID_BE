using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Transaction
{
    public class TransactionRequest
    {
        public double Amount { get; set; }

        public string MethodId { get; set; }
    }
}
