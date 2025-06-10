using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.Helpers
{
    public static class HybridVariables
    {
        public static string JwtSecret => Environment.GetEnvironmentVariable("Jwt__Secret") ?? "";
        public static string ConnectionString => Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ?? "";
        public static string SecretString => Environment.GetEnvironmentVariable("SecretString") ?? "";
        public static string ClientId => Environment.GetEnvironmentVariable("ClientID") ?? "";
        public static string ClientSecret => Environment.GetEnvironmentVariable("ClientSecret") ?? "";
        public static string GmailEmail => Environment.GetEnvironmentVariable("GMAIL_EMAIL") ?? "";
        public static string GmailAppPassword => Environment.GetEnvironmentVariable("GMAIL_APP_PASSWORD") ?? "";
        public static int ResetCodeLength => int.Parse(Environment.GetEnvironmentVariable("RESET_CODE_LENGTH")!);
        public static string PayOsClientId => Environment.GetEnvironmentVariable("PayOs__ClientId") ?? "";
        public static string PayOsApiKey => Environment.GetEnvironmentVariable("PayOs__ApiKey") ?? "";
        public static string PayOsCheckSumKey => Environment.GetEnvironmentVariable("PayOs__ChecksumKey") ?? "";
    }
}
