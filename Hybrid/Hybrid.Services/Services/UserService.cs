using Azure.Core;
using Google.Apis.Auth;
using Hybrid.Repositories.Base;
using Hybrid.Repositories.Models;
using Hybrid.Repositories.Repos;
using Hybrid.Services.Helpers;
using Hybrid.Services.ViewModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.Services
{
    public interface IUserService
    {
        Task<User?> Authenticate(LoginRequest loginRequest);
        Task<User?> AuthenticateGoogle(string token);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<int> UpdateUserAccount(User user);
        Task<(bool, string)> ResetPasswordAsync(string email);
    }

    public class UserService : IUserService
    {
        private readonly UserRepository _userRepo;

        public UserService(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        /// <summary>
        /// FUNC_Authenticate
        /// LoginRequest_ViewModel
        /// User?
        /// Created By: TriNHM
        /// Created Date: 13/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<User?> Authenticate(LoginRequest loginRequest)
        {
            var hashedPassword = Sha256Encoding.ComputeSHA256Hash(loginRequest.Password + HybridVariables.SecretString);

            return await _userRepo.GetUserAsync(loginRequest.Email, hashedPassword);
        }

        /// <summary>
        /// FUNC_AuthenticateGoogle
        /// Created By: TuanCASE
        /// Created Date: 20/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<User?> AuthenticateGoogle(string token)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { HybridVariables.ClientId }
                });
            }
            catch (InvalidJwtException)
            {
                return null;
            }

            var user = await _userRepo.GetUserByMailAsync(payload.Email);

            if (user != null)
            {
                return user;
            }

            // Create new user for Gmail email
            var newUser = new User()
            {
                Email = payload.Email,
                Password = Sha256Encoding.ComputeSHA256Hash(payload.Email + HybridVariables.SecretString + payload.EmailVerified),
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                RoleId = "1",
            };
            await _userRepo.CreateAsync(newUser);   
            newUser = await _userRepo
                .GetFirstWithIncludeAsync(x => x.UserId == newUser.UserId, [x => x.Role]);

            return newUser;
        }

        /// <summary>
        /// FUNC_GetUserByRefreshTokenAsync
        /// refreshToken_string
        /// User?
        /// Created By: TriNHM
        /// Created Date:14/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _userRepo.GetByRefreshTokenAsync(refreshToken);
        }

        /// <summary>
        /// FUNC_ResetPassword
        /// Created By: TuanCASE
        /// Created Date: 20/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<(bool, string)> ResetPasswordAsync(string email)
        {
            (bool, string) result = new(false, string.Empty);
            var user = await _userRepo.GetUserByMailAsync(email);

            if (user == null)
            {
                result.Item2 = "User not found with this email.";
                return result;
            }

            EmailSender.SendPasswordReset(user.Email);

            result.Item1 = true;
            result.Item2 = $"Reset code successfully sent to: {user.Email}";
            return result;
        }

        /// <summary>
        /// FUNC_UpdateUserAccount
        /// User
        /// int
        /// Created By: TriNHM
        /// Created Date:14/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<int> UpdateUserAccount(User user)
        {
            return await _userRepo.UpdateAsync(user);
        }
    }
}
