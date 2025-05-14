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
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<int> UpdateUserAccount(User user);
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
            var hashedPassword = Sha256Encoding.ComputeSHA256Hash(loginRequest.Password + Environment.GetEnvironmentVariable("SecretString"));

            return await _userRepo.GetUserAsync(loginRequest.Email, hashedPassword);
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
