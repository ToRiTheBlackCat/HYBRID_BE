using Hybrid.Repositories.Models;
using Hybrid.Services.Services;
using Hybrid.Services.ViewModel.Rating;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hybrid.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        /// <summary>
        /// API_GetAllRatingOfMinigame
        /// minigameId - string?
        /// List<Rating>
        /// Created By: TriNHM
        /// Created Date: 8/6/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpGet("{minigameId}")]
        public async Task<ActionResult<List<GetAllRatingResponse>>> GetAllRatingOfMinigame(string minigameId)
        {
            var ratingList = await _ratingService.GetAllRatingByMinigameId(minigameId);

            return Ok(ratingList);
        }

        /// <summary>
        /// API_RatingMinigame
        /// CreateRatingRequest_ViewModel
        /// Rating
        /// Created By: TriNHM
        /// Created Date: 8/6/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Rating>> RatingMinigame(CreateRatingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (isSuccess, message) = await _ratingService.CreateRating(request);

            return Ok(new
            {
                isSuccess,
                message
            });
        }

        /// <summary>
        /// API_GetScoreOfMinigame
        /// minigameId_string
        /// RatingScore
        /// Created By: TriNHM
        /// Created Date: 8/6/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpGet("score-{minigameId}")]
        public async Task<ActionResult> GetScoreOfMinigame(string minigameId)
        {
            var ratingScore = await _ratingService.GetScoreOfMinigame(minigameId);
            return Ok(new
            {
                RatingScore = ratingScore
            });
        }
    }
}
