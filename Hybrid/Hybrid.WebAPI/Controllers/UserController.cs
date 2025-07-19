using Azure.Core;
using Hybrid.Services.Services;
using Hybrid.Services.ViewModel.Profile;
using Hybrid.Services.ViewModel.SignUp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Hybrid.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// API_SignUp 
        /// SignUpRequest_ViewModel
        /// X
        /// Created By: TriNHM
        /// Created Date: 21/5/2025
        /// Updated By: TriNHM
        /// Updated Date: 28/5/2025
        /// </summary>
        [HttpPost("signup")]
        public async Task<ActionResult> SignUpUser([FromBody] SignUpUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (isSuccess, userId, message) = await _userService.SignUpUserAccount(request);

            return Ok(new
            {
                isSuccess,
                userId,
                message
            });
        }

        /// <summary>
        /// API_SignUpStudent 
        /// SignUpTeacher_StudentRequest_ViewModel
        /// X
        /// Created By: TriNHM
        /// Created Date: 28/5/2025
        /// Updated By: 
        /// Updated Date: 
        /// </summary>
        [HttpPost("signup-student")]
        public async Task<ActionResult> SignUpStudent([FromBody] SignUpTeacher_StudentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (isSuccess, message) = await _userService.SignUpStudentAccount(request);

            return Ok(new
            {
                isSuccess,
                message
            });
        }

        /// <summary>
        /// API_SignUpTeacher 
        /// SignUpTeacher_StudentRequest_ViewModel
        /// X
        /// Created By: TriNHM
        /// Created Date: 28/5/2025
        /// Updated By: 
        /// Updated Date: 
        /// </summary>
        [HttpPost("signup-teacher")]
        public async Task<ActionResult> SignUpTeacher([FromBody] SignUpTeacher_StudentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (isSuccess, message) = await _userService.SignUpTeacherAccount(request);

            return Ok(new
            {
                isSuccess,
                message
            });
        }

        /// <summary>
        /// API_UpdateProfile
        /// Created By: TuanCA
        /// Created Date: 23/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpGet("profile")]
        public async Task<ActionResult> GetProfile([FromQuery] GetProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.GetProfileAsync(request);
            if (result == null)
            {
                return NotFound("No " + (request.IsTeacher ? "Teacher" : "Student") + " profile found.");
            }

            return Ok(result);
        }

        /// <summary>
        /// API_UpdateProfile
        /// Created By: TuanCA
        /// Created Date: 22/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("update-profile")]
        [Authorize]
        public async Task<ActionResult<UpdateProfileResponse?>> UpdateProfile([FromBody] UpdateProfileRequest updateRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateProfileAsync(updateRequest);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        [HttpPost("update-role")]
        [Authorize]
        public async Task<ActionResult<UpdateProfileResponse?>> UpdateRole(bool isTeacher)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get teacherId in auth token
            var clainmTeacherId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

            var (isSuccess, message) = await _userService.UpdateUserRole(clainmTeacherId, isTeacher);
            if (!isSuccess)
            {
                return BadRequest(message);
            }

            return Ok(new
            {
                isSuccess,
                message
            });
        }
    }
}
