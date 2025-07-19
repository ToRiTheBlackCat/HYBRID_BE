using Azure.Core;
using Google.Apis.Auth;
using Hybrid.Repositories.Base;
using Hybrid.Repositories.Models;
using Hybrid.Repositories.Repos;
using Hybrid.Services.Constant;
using Hybrid.Services.Helpers;
using Hybrid.Services.ViewModel.Course;
using Hybrid.Services.ViewModel.Login;
using Hybrid.Services.ViewModel.Others;
using Hybrid.Services.ViewModel.Profile;
using Hybrid.Services.ViewModel.SignUp;
using Hybrid.Services.ViewModel.User;
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
        //Task<User?> AuthenticateGoogle(string token, string roleId);
        Task<User?> AuthenticateGoogle(SignupUserGoogleRequest request);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<User?> GetUserByEmailAsync(string email);
        Task<int> UpdateUserAccount(User user);
        Task<(bool, string, string)> SignUpUserAccount(SignUpUserRequest request);
        Task<(bool, string)> SignUpTeacherAccount(SignUpTeacher_StudentRequest request);
        Task<(bool, string)> SignUpStudentAccount(SignUpTeacher_StudentRequest request);
        Task<User?> GetUserByIdAsync(string userId);
        Task<(bool, string)> RequestResetPasswordAsync(string email);
        Task<(bool, string)> ConfirmResetPasswordAsync(ConfirmResestRequest resestRequest);
        Task<GetProfileResponse?> GetProfileAsync(GetProfileRequest request);
        Task<UpdateProfileResponse> UpdateProfileAsync(UpdateProfileRequest request);
        Task<AnalyzeUserResponse> AnalyzeUser();
        Task<List<FilterUsersResponse>> FilterUsersByCreatedDate(DateTime? fromDate, DateTime? toDate);
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
        //public async Task<User?> AuthenticateGoogle(string token, string roleId)
        public async Task<User?> AuthenticateGoogle(SignupUserGoogleRequest request)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(request.Token, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { HybridVariables.ClientId }
                });
            }
            catch (InvalidJwtException ex)
            {
                return null;
            }

            var user = await _userRepo.GetUserByMailAsync(payload.Email);

            if (user != null)
            {
                if (user.RoleId != request.RoleId)
                {
                    return null;
                }

                return user;
            }

            // Create new user for Gmail email
            var newUser = new User()
            {
                Email = payload.Email,
                Password = Sha256Encoding.ComputeSHA256Hash(payload.Email + HybridVariables.SecretString + payload.EmailVerified),
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                RoleId = request.RoleId,
            };
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _userRepo.CreateAsync(newUser);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

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
        /// FUNC_GetUserById
        /// Created By: TuanCA
        /// Created Date:22/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepo
                .GetFirstWithIncludeAsync(x => x.Email.Equals(email), x => x.Role);
        }

        /// <summary>
        /// FUNC_GetUserByIdAsync
        /// userId_string
        /// User?
        /// Created By: TriNHM
        /// Created Date: 22/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<User?> GetUserByIdAsync(string userId)
        {
            return await _unitOfWork.UserRepo.GetByIdAsync(userId);
        }

        /// <summary>
        /// FUNC_ResetPassword
        /// Created By: TuanCASE
        /// Created Date: 20/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<(bool, string)> RequestResetPasswordAsync(string email)
        {
            (bool, string) result = new(false, string.Empty);
            var user = await _userRepo.GetUserByMailAsync(email);

            if (user == null)
            {
                result.Item2 = "User not found with this email.";
                return result;
            }

            // Attempts to create reset code
            try
            {
                var resetCode = EmailSender.SendPasswordReset(user.Email); // Send to email

                await _unitOfWork.BeginTransactionAsync();

                user.ResetCode = Sha256Encoding.ComputeSHA256Hash(HybridVariables.SecretString + resetCode);
                await _unitOfWork.UserRepo.UpdateAsync(user);

                await _unitOfWork.CommitTransactionAsync();
                result.Item1 = true;
                result.Item2 = $"Reset code successfully sent to: {user.Email}";
                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                result.Item2 = "Failed to send/create Password reset code. " + ex.InnerException;
            }

            return result;
        }

        /// <summary>
        /// FUNC_ResetPassword
        /// Created By: TuanCASE
        /// Created Date: 23/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<(bool, string)> ConfirmResetPasswordAsync(ConfirmResestRequest resestRequest)
        {
            var result = (false, "");
            var userRepo = _unitOfWork.UserRepo;
            resestRequest.ResetCode = Sha256Encoding.ComputeSHA256Hash(HybridVariables.SecretString + resestRequest.ResetCode);

            var user = await userRepo.GetFirstWithIncludeAsync(x =>
                x.Email.Equals(resestRequest.Email) &&
                x.ResetCode == resestRequest.ResetCode
            );

            if (user != null)
            {
                try
                {
                    await _unitOfWork.BeginTransactionAsync();

                    user.Password = Sha256Encoding.ComputeSHA256Hash(resestRequest.Password + HybridVariables.SecretString);
                    await userRepo.UpdateAsync(user);

                    await _unitOfWork.CommitTransactionAsync();
                    result = (true, "Password reset successful.");
                    return result;
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    result.Item2 = "Password reset failed: " + ex.InnerException;
                }
            }
            else
            {
                result.Item2 = "Reset code is invalid.";
            }

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
        /// Updated By: TriNHM
        /// Updated Date: 28/5/2025
        /// </summary>
        public async Task<(bool, string, string)> SignUpUserAccount(SignUpUserRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var hashedPassword = Sha256Encoding.ComputeSHA256Hash(request.Password + HybridVariables.SecretString);
                request.Password = hashedPassword;

                var newUser = request.Map_SignUpUserVM_To_User();
                await _unitOfWork.UserRepo.CreateAsync(newUser);
                await _unitOfWork.CommitTransactionAsync();

                return (true, newUser.UserId, "SignUp successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.Write(ex.Message);
                return (false, "", "SignUp fail");
            }
        }

        /// <summary>
        /// FUNC_SignUpTeacherAccount
        /// Created By: TriNHM
        /// Created Date: 28/5/2025
        /// Updated By: 
        /// Updated Date: 
        /// </summary>
        public async Task<(bool, string)> SignUpTeacherAccount(SignUpTeacher_StudentRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var newTeacher = request.Map_SignUpTeacher_StudentRequestVM_To_Teacher();
                await _unitOfWork.TeacherRepo.CreateAsync(newTeacher);
                await _unitOfWork.CommitTransactionAsync();

                return (true, "SignUp Teacher account successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.Write(ex.Message);
                return (false, "SignUp Teacher account fail");
            }
        }

        /// <summary>
        /// FUNC_SignUpStudentAccount
        /// Created By: TriNHM
        /// Created Date: 28/5/2025
        /// Updated By: 
        /// Updated Date: 
        /// </summary>
        public async Task<(bool, string)> SignUpStudentAccount(SignUpTeacher_StudentRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var newStudent = request.Map_SignUpTeacher_StudentRequestVM_To_Student();
                await _unitOfWork.StudentRepo.CreateAsync(newStudent);
                await _unitOfWork.CommitTransactionAsync();

                return (true, "SignUp Student account successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.Write(ex.Message);
                return (false, "SignUp Student account fail");
            }
        }


        /// <summary>
        /// FUNC_SignUpUserAccount
        /// Created By: TuanCA
        /// Created Date: 23/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<GetProfileResponse?> GetProfileAsync(GetProfileRequest request)
        {
            var user = await _unitOfWork.UserRepo.GetFirstWithIncludeAsync(
                x => x.UserId == request.UserId
            );

            if (user == null)
            {
                return null;
            }

            GetProfileResponse? response;
            if (request.IsTeacher)
            {
                var teacher = await _unitOfWork.TeacherRepo.GetFirstWithIncludeAsync(
                    x => x.UserId == request.UserId,
                    x => x.Tier
                );

                response = teacher?.ToGetProfileResponse();

            }
            else
            {
                var student = await _unitOfWork.StudentRepo.GetFirstWithIncludeAsync(
                    x => x.UserId == request.UserId,
                    x => x.Tier
                );

                response = student?.ToGetProfileResponse();
            }

            if (response != null)
            {
                response.Email = user.Email;
            }

            return response;
        }

        /// <summary>
        /// FUNC_SignUpUserAccount
        /// Created By: TuanCA
        /// Created Date: 23/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<UpdateProfileResponse> UpdateProfileAsync(UpdateProfileRequest request)
        {
            var result = new UpdateProfileResponse();
            result.Success = false;

            var user = await _unitOfWork.UserRepo.GetFirstWithIncludeAsync(x =>
                x.UserId == request.UserId
            );

            if (user == null)
            {
                result.Message = "User not found.";
                return result;
            }

            await _unitOfWork.BeginTransactionAsync();

            if (request.IsTeacher)
            {
                var teacher = await _unitOfWork.TeacherRepo.GetFirstWithIncludeAsync(
                    x => x.UserId == request.UserId,
                    x => x.Tier
                );

                if (teacher == null)
                {
                    result.Message = "User have no Teacher account";
                    return result;
                }

                request.AssignValueTo(teacher);
                _unitOfWork.TeacherRepo.Update(teacher);
                result.ProfileView = teacher?.ToUpdateProfileView();
            }
            else
            {
                var student = await _unitOfWork.StudentRepo.GetFirstWithIncludeAsync(
                    x => x.UserId == request.UserId,
                    x => x.Tier
                );

                if (student == null)
                {
                    result.Message = "User have no Student account";
                    return result;
                }

                request.AssignValueTo(student);
                _unitOfWork.StudentRepo.Update(student);
                result.ProfileView = student?.ToUpdateProfileView();
            }

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();
            result.Success = true;
            result.Message = "Account " + (request.IsTeacher ? "Teacher" : "Student") + " profile updated successfully.";

            return result;
        }

        public async Task<AnalyzeUserResponse> AnalyzeUser()
        {
            try
            {
                var (numberOfStudent, numberOfTeacher) = await _unitOfWork.UserRepo.GetUsersCountByRoleAsync();
                var (studentList, teacherList) = await _unitOfWork.UserRepo.GetUsersListByRoleAsync();
                return new AnalyzeUserResponse
                {
                    NumberOfStudents = numberOfStudent,
                    StudentsList = studentList,
                    NumbersOfTeacher = numberOfTeacher,
                    TeachersList = teacherList
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new AnalyzeUserResponse();
            }
        }

        public async Task<List<FilterUsersResponse>> FilterUsersByCreatedDate(DateTime? fromDate, DateTime? toDate)
        {
            var list = (await _unitOfWork.UserRepo.GetAllAsync()).AsQueryable();

            if (fromDate != null && toDate == null)
            {
                list = list.Where(x => x.CreatedDate >= fromDate);
            }
            else if (fromDate == null && toDate != null)
            {
                list = list.Where(x => x.CreatedDate <= toDate);
            }
            else if (fromDate != null && toDate != null)
            {
                list = list.Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate);
            }
            var mappedList = (list.ToList()).Map_ListUser_To_ListFilterUsersResponse();

            return mappedList;
        }



    }
}
