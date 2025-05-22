using Hybrid.Services.Services;
using Hybrid.Services.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("signup")]
        public async Task<ActionResult> SignUp([FromBody] SignUpRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (isSuccess, message) = await _userService.SignUpUserAccount(request);

            return Ok(new
            {
                isSuccess,
                message
            });
        }
    }
}
