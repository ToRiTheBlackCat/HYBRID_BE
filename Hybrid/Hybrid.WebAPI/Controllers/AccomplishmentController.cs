using Hybrid.Services.Services;
using Hybrid.Services.ViewModel.Accomplishment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Hybrid.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccomplishmentController : ControllerBase
    {
        private readonly IStudentAccomplishmentService _accomplishmentService;

        public AccomplishmentController(IStudentAccomplishmentService accomplishmentService)
        {
            _accomplishmentService = accomplishmentService;
        }

        /// <summary>
        /// API - Get Accomplishments by StudentId and MinigameId
        /// Created By: TuanCA
        /// Created Date: 17/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpGet("{minigameId}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<AccomplishmentViewModel>>> GetAction([Required] string minigameId, bool getSelf = true)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentId = getSelf ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value: string.Empty;
            if (getSelf && string.IsNullOrEmpty(studentId))
            {
                ModelState.AddModelError("StudentId", "User is not logged-in.");
                return BadRequest(ModelState);
            }

            var accomplishments = await _accomplishmentService.GetAccomplishmentsAsync(studentId, minigameId);
            if (accomplishments == null || !accomplishments.Any())
            {
                return NotFound("No accomplishments found for the given student and minigame.");
            }
            return Ok(accomplishments);
        }

        /// <summary>
        /// API - Post Accomplishment
        /// Created By: TuanCA
        /// Created Date: 17/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<AddAccomplishmentResponse>> PostAccomplishment([FromBody] AddAccomplishmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            request.StudentId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var response = await _accomplishmentService.AddAccomplishmentAsync(request);
            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }
    }
}
