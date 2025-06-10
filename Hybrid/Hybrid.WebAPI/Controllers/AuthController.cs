using Azure.Core;
using Hybrid.Repositories.Base;
using Hybrid.Services.Helpers;
using Hybrid.Services.Services;
using Hybrid.Services.ViewModel.Login;
using Hybrid.Services.ViewModel.Others;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Hybrid.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtAuthentication _jwtAuth;
        private readonly IUserService _userService;

        public AuthController(JwtAuthentication jwtAuth,
                              IUserService userService)
        {
            _jwtAuth = jwtAuth;
            _userService = userService;
        }


        /// <summary>
        /// API_Login
        /// LoginRequest_ViewModel
        /// LoginResponse_ViewModel
        /// Created By: TriNHM
        /// Created Date: 13/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse?>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.Authenticate(request);
            if (user == null)
            {
                return NotFound("User not found. Try again!");
            }
            else
            {
                var accessToken = _jwtAuth.GenerateAccessToken(user);
                var refreshToken = _jwtAuth.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _userService.UpdateUserAccount(user);

                var response = new LoginResponse
                {
                    UserId = user.UserId.Trim(),
                    RoleId = user.RoleId,
                    RoleName = user.Role.RoleName,
                    AccessToken = accessToken,
                    RefreshToken = user.RefreshToken,
                };

                return Ok(response);
            }
        }

        /// <summary>
        /// API_GoogleLogin 
        /// CreatedBy: TuanCA
        /// Created Date: 20/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("login-google")]
        public async Task<ActionResult<LoginResponse?>> LoginGoogle([FromBody] string token)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.AuthenticateGoogle(token);
            if (user == null)
            {
                return NotFound("User not found. Try again!");
            }
            else
            {
                var accessToken = _jwtAuth.GenerateAccessToken(user);
                var refreshToken = _jwtAuth.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _userService.UpdateUserAccount(user);

                var response = new LoginResponse
                {
                    UserId = user.UserId.Trim(),
                    RoleId = user.RoleId,
                    RoleName = user.Role.RoleName,
                    AccessToken = accessToken,
                    RefreshToken = user.RefreshToken,
                };

                return Ok(response);
            }
        }

        /// <summary>
        /// API_PasswordReset
        /// CreatedBy: TuanCA
        /// Created Date: 20/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("request-reset")]
        public async Task<ActionResult> RequestPasswordReset([FromBody][EmailAddress] string email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RequestResetPasswordAsync(email);
            return Ok(new
            {
                Success = result.Item1,
                Message = result.Item2
            });
        }

        /// <summary>
        /// API_ConfirmPasswordReset
        /// CreatedBy: TuanCA
        /// Created Date: 25/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("confirm-reset")]
        public async Task<ActionResult<LoginResponse?>> ConfirmPasswordReset([FromBody] ConfirmResestRequest resestRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.GetUserByEmailAsync(resestRequest.Email);

            if (user == null)
            {
                return NotFound($"User with email {resestRequest.Email} was not found.");
            }

            var result = await _userService.ConfirmResetPasswordAsync(resestRequest);

            return Ok(new
            {
                Success = result.Item1,
                Message = result.Item2
            });
        }

        /// <summary>
        /// API_Refresh Token 
        /// refreshToken_string
        /// RefreshTokenResponse_ViewModel
        /// Created By: TriNHM
        /// Created Date: 14/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var user = await _userService.GetUserByRefreshTokenAsync(refreshToken);

            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                return Unauthorized("Invalid or expired refresh token");

            var response = _jwtAuth.RefreshTokenAsync(user);

            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userService.UpdateUserAccount(user);

            return Ok(response);
        }
    }
}
