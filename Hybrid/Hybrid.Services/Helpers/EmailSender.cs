using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.Helpers
{
    public static class EmailSender
    {
        public static void SendPasswordReset(string toEmail)
        {
            var email = HybridVariables.GmailEmail ?? "";
            var password = HybridVariables.GmailAppPassword ?? "";
            
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(email, password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(email),
                Subject = "Hybrid E-learning Password Reset.",
                Body = $"This is the code for reseting the password: {GenerateSecureRandomString()}",
                IsBodyHtml = false,
            };

            mailMessage.To.Add(toEmail);

            smtpClient.Send(mailMessage);
        }

        private static string GenerateSecureRandomString()
        {
            var length = int.Parse(HybridVariables.ResetCodeLength!);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            using var rng = RandomNumberGenerator.Create();
            var result = new char[length];
            var buffer = new byte[sizeof(uint)];

            for (int i = 0; i < length; i++)
            {
                rng.GetBytes(buffer);
                uint num = BitConverter.ToUInt32(buffer, 0);
                result[i] = chars[(int)(num % (uint)chars.Length)];
            }

            return new string(result);
        }

    }
}
