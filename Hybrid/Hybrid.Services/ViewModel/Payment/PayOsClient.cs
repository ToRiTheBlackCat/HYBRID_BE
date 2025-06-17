using Hybrid.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Payment
{
    public class PayOsClient
    {
        public string ClientId { get; set; } = HybridVariables.PayOsClientId;
        public string ApiKey { get; set; } = HybridVariables.PayOsApiKey;
        public string ChecksumKey { get; set; } = HybridVariables.PayOsCheckSumKey;
    }
}
