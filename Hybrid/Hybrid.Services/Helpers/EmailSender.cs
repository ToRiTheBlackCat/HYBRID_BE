using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.Helpers
{
    public static class EmailSender
    {
        public static string SendPasswordReset(string toEmail)
        {
            var email = HybridVariables.GmailEmail ?? "";
            var password = HybridVariables.GmailAppPassword ?? "";
            
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(email, password),
                EnableSsl = true,
            };

            var resetCode = GenerateSecureRandomString();
            var mailMessage = new MailMessage
            {
                From = new MailAddress(email),
                Subject = "🔐 HYBRID E-LEARNING PASSWORD RESET 🔐",
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            string htmlBody = $@"
<html>
  <body style='margin: 0; padding: 0; background-color: #ffffff; font-family: Arial, sans-serif;'>
    <table width='100%' cellpadding='0' cellspacing='0' style='padding: 40px 0;'>
      <tr>
        <td align='center'>
          <table width='600' cellpadding='0' cellspacing='0' style='background-color: #1e2a38; border-radius: 10px; padding: 40px; text-align: center;'>
            <tr>
              <td>
                <img src='cid:LockImage' alt='Lock Icon' style='width: 100px; margin-bottom: 30px;' />
                <h2 style='color: #ffffff; font-size: 24px; margin-bottom: 20px;'>Forgot your password?</h2>
                <p style='color: #ffffff; font-size: 16px; margin-bottom: 30px;'>
                  If you've lost your password or wish to reset it, use the code below:
                </p>
                <div style='font-size: 24px; font-weight: bold; background-color: #00ffff; color: white; display: inline-block; padding: 10px 20px; border-radius: 5px; margin-bottom: 30px;'>
                  {resetCode}
                </div>
                <p style='color: #ffffff; font-size: 14px;'>
                  Use this code in the app to complete your password reset.
                </p>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </body>
</html>";


            AlternateView avHtml = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);

            // Dynamic path to the image in wwwroot/images/logo/logo.jpg
            string imagePath = Path.Combine("wwwroot", "images", "logo", "logo.jpg");



            LinkedResource inlineImage = new LinkedResource(imagePath, MediaTypeNames.Image.Png)
            {
                ContentId = "LockImage",
                ContentType = new ContentType(MediaTypeNames.Image.Png),
                TransferEncoding = TransferEncoding.Base64,
                ContentLink = new Uri("cid:LockImage")
            };
            avHtml.LinkedResources.Add(inlineImage);

            mailMessage.AlternateViews.Add(avHtml);

            smtpClient.Send(mailMessage);

            return resetCode;
        }

        private static string GenerateSecureRandomString()
        {
            var length = HybridVariables.ResetCodeLength;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
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
