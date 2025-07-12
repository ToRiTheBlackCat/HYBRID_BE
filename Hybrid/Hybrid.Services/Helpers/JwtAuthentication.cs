using Hybrid.Repositories.Models;
using Hybrid.Repositories.Repos;
using Hybrid.Services.ViewModel.Others;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.Helpers
{
    public class JwtAuthentication
    {
        private readonly IConfiguration _configure;
        public JwtAuthentication(IConfiguration configure)
        {
            _configure = configure;
        }

        /// <summary>
        /// FUNC_GenerateAccessToken
        /// User
        /// string
        /// Created By: TriNHM
        /// Created Date: 13/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public string GenerateAccessToken(User? user)
        {
            var jwtKey = HybridVariables.JwtSecret;
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? ""));
            var credential = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.Trim()),
                new Claim(ClaimTypes.Role, user.RoleId.Trim()),
            };

            var token = new JwtSecurityToken(
                issuer: _configure["JWT:Issuer"],
                audience: _configure["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credential
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return accessToken;
        }

        /// <summary>
        /// FUNC_GenerateRefreshToken
        /// ...
        /// string
        /// Created By: TriNHM
        /// Created Date: 14/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public string GenerateRefreshToken()
        {
            var bytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// FUNC_RefreshTokenAsync
        /// User
        /// RefreshTokenResponse_ViewModel
        /// Created By: TriNHM
        /// Created Date: 14/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public RefreshTokenResponse RefreshTokenAsync(User user)
        {
            var newAccessToken = GenerateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken();

            return new RefreshTokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
