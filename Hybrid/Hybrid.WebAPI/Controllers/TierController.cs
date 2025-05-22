using Hybrid.Services.Services;
using Hybrid.Services.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hybrid.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TierController : ControllerBase
    {
        private readonly ITierService _tierService;
        private readonly IUserService _userService;
        public TierController(ITierService tierService, IUserService userService)
        {
            _tierService = tierService;
            _userService = userService;
        }

        /// <summary>
        /// API_GetAllTierOfStudent
        /// X
        /// List of StudentTier
        /// Created By: TriNHM
        /// Created Date: 21/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpGet("tier-student")]
        public async Task<ActionResult> GetAllTierOfStudent()
        {
            var tierList = await _tierService.GetAllTierOfStudent();
            return Ok(tierList);
        }

        /// <summary>
        /// API_GetDetailOfStudentTierById
        /// tierId_string
        /// TierResponse_ViewModel
        /// Created By: TriNHM
        /// Created Date: 21/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpGet("tier-student-{tierId}")]
        public async Task<ActionResult<TierResponse>> GetDetailOfStudentTierById(string tierId)
        {
            if (string.IsNullOrWhiteSpace(tierId))
                return BadRequest("TierId is missing!");

            var tier = await _tierService.GetTierOfStudentById(tierId);
            return Ok(tier);
        }

        /// <summary>
        /// API_GetAllTierOfTeacher
        /// X
        /// List of TeacherTier
        /// Created By: TriNHM
        /// Created Date: 21/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpGet("tier-teacher")]
        public async Task<ActionResult> GetAllTierOfTeacher()
        {
            var tierList = await _tierService.GetAllTierOfTeacher();
            return Ok(tierList);
        }

        /// <summary>
        /// API_GetDetailOfTeacherTierById
        /// tierId_string
        /// TierResponse_ViewModel
        /// Created By: TriNHM
        /// Created Date: 21/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpGet("tier-teacher-{tierId}")]
        public async Task<ActionResult<TierResponse>> GetDetailOfTeacherTierById(string tierId)
        {
            if (string.IsNullOrWhiteSpace(tierId))
                return BadRequest("TierId is missing!");

            var tier = await _tierService.GetTierOfTeacherById(tierId);
            return Ok(tier);
        }

        /// <summary>
        /// API_UpgradeTier
        /// UpgradeTierRequest_ViewModel
        /// TierResponse_ViewModel
        /// Created By: TriNHM
        /// Created Date: 22/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("upgrade")]
        public async Task<ActionResult> UpgradeTier([FromBody] UpgradeTierRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (isSuccess, message) = await _tierService.UpgradeTierOfUser(request);
            return Ok(new
            {
                isSuccess,
                message
            });
        }
    }
}
