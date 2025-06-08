using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.SupscriptionExtention
{
    public class SupscriptionExtentionOrderRequest
    {
        public string UserId { get; set; }

        public string TierId { get; set; }

        public string TransactionId { get; set; }

        public int Days { get; set; }
    }
}
