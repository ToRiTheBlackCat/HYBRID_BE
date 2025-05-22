using Azure.Core;
using Google.Apis.Auth;
using Hybrid.Repositories.Base;
using Hybrid.Repositories.Models;
using Hybrid.Repositories.Repos;
using Hybrid.Services.Constant;
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
        Task<(bool, string)> SignUpUserAccount(SignUpRequest request);

    }

    public class UserService : IUserService
    {
        private UserRepository _userRepo => _unitOfWork.UserRepo;
        private readonly UnitOfWork _unitOfWork;

        public UserService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        /// <summary>
        /// FUNC_SignUpUserAccount
        /// Created By: TriNHM
        /// Created Date: 21/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<(bool, string)> SignUpUserAccount(SignUpRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var hashedPassword = Sha256Encoding.ComputeSHA256Hash(request.Password + HybridVariables.SecretString);
                request.Password = hashedPassword;

                var newUser = request.Map_SignUpVM_To_User();

                await _unitOfWork.UserRepo.CreateAsync(newUser);

                if (request.RoleId.Equals(ConstantEnum.RoleID.STUDENT))
                {
                    var newStudent = request.Map_SignUpVM_To_Student();
                    newStudent.UserId = newUser.UserId;
                    await _unitOfWork.StudentRepo.CreateAsync(newStudent);

                }
                else if (request.RoleId.Equals(ConstantEnum.RoleID.TEACHER))
                {
                    var newTeacher = request.Map_SignUpVM_To_Teacher();
                    newTeacher.UserId = newUser.UserId;
                    await _unitOfWork.TeacherRepo.CreateAsync(newTeacher);
                }

                await _unitOfWork.CommitTransactionAsync();
                return (true, "SignUp successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.Write(ex.Message);
                return (false, "SignUp fail");
            }
        }
    }
}
