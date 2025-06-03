using Azure;
using Hybrid.Services.Services;
using Hybrid.Services.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hybrid.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiniGameController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IMiniGameService _miniGameService;
        public MiniGameController(IWebHostEnvironment env, IMiniGameService miniGameService)
        {
            _env = env;
            _miniGameService = miniGameService;
        }

        //[HttpGet]
        //public async Task<ActionResult> GetMinigamesOfCoure(string courseId)
        //{
        //    throw
        //}

        /// <summary>
        /// API_Get MiniGame of Teacher
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<GetAllMinigameResponse>> GetMiniGameOfTeacher(string teacherId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _miniGameService.GetMinigameOfTeacherAsync(teacherId);
            if (response == null)
            {
                return NotFound("Minigame not found.");
            }

            return Ok(response);
        }

        /// <summary>
        /// API_Get MiniGame
        /// Created By: TuanCA
        /// Created Date: 26/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetMiniGameResponse>> GetMiniGame([Required] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _miniGameService.GetMiniGameAsync(id);
            if (response == null)
            {
                return NotFound("Minigame not found.");
            }

            return Ok(response);
        }

        /// <summary>
        /// API_Add Conjunction Minigame
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("conjunction")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddMiniGame([FromForm] AddMiniGameRequest<ConjunctionQuestion> request)
        {
            return await AddMiniGame<ConjunctionQuestion>(request);
        }

        /// <summary>
        /// API_Add Quiz Minigame
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("quiz")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddMiniGame([FromForm]AddMiniGameRequest<QuizQuestion> request)
        {
            return await AddMiniGame<QuizQuestion>(request);
        }

        /// <summary>
        /// API_Add Anagram Minigame
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("anagram")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddMiniGame([FromForm]AddMiniGameRequest<AnagramQuestion> request)
        {
            return await AddMiniGame<AnagramQuestion>(request);
        }

        /// <summary>
        /// Generic method to handle adding minigames
        /// </summary>
        private async Task<IActionResult> AddMiniGame<T>([FromForm] AddMiniGameRequest<T> request) where T : MinigameModels 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!request.GameData.Any())
            {
                ModelState.AddModelError(nameof(request.GameData), "The GameData field must have atleast one item.");
                return BadRequest(ModelState);
            }

            var result = await _miniGameService.AddMiniGameAsync(request, Path.GetExtension(request.ImageFile.FileName));
            if (!result.IsSuccess)
            {
                return Ok(result);
            }

            var _ = SaveImage(request.ImageFile, result.Model!.ThumbnailImage);

            return Ok(result);
        }

        /// <summary>
        /// API_Add Conjunction Minigame
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPut("conjunction")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateMiniGame([FromForm] UpdateMinigameRequest<ConjunctionQuestion> request)
        {
            return await UpdateMiniGame<ConjunctionQuestion>(request);
        }

        /// <summary>
        /// API_Add Quiz Minigame
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPut("quiz")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateMiniGame([FromForm] UpdateMinigameRequest<QuizQuestion> request)
        {
            return await UpdateMiniGame<QuizQuestion>(request);
        }

        /// <summary>
        /// API_Add Anagram Minigame
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPut("anagram")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateMiniGame([FromForm] UpdateMinigameRequest<AnagramQuestion> request)
        {
            return await UpdateMiniGame<AnagramQuestion>(request);
        }

        /// <summary>
        /// Generic method to handle updating minigames
        /// </summary>
        /// <param name="fakeTeacherId">TeacherId for testing without Authentication</param>
        /// <returns></returns>
        private async Task<IActionResult> UpdateMiniGame<T>(UpdateMinigameRequest<T> request, string fakeTeacherId = "") where T : MinigameModels
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Checking request's actual teacher ID from claims, if not provided in request
            if (string.IsNullOrEmpty(fakeTeacherId))
            {
                var clainmTeacherId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
                request.SetTeacherId(clainmTeacherId);
            }
            else
            {
                request.SetTeacherId(fakeTeacherId);
            }

            if (!request.GameData.Any())
            {
                ModelState.AddModelError(nameof(request.GameData), "The GameData field must have at least one item.");
                return BadRequest(ModelState);
            }

            var result = await _miniGameService.UpdateMiniGameAsync(request, Path.GetExtension(request.ImageFile.FileName));

            if (!result.IsSuccess)
            {
                return Ok(result);
            }
            var _ = SaveImage(request.ImageFile, result.Model!.ThumbnailImage);

            return Ok(result);
        }

        /// <summary>
        /// Func_Save image
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        private string SaveImage(IFormFile imageFile, string saveName)
        {
            string saveFolder = Path.Combine(_env.WebRootPath, "images", "users");
            Directory.CreateDirectory(saveFolder); // Ensure the folder exists

            //string fileName = $"{saveName}{Path.GetExtension(imageFile.FileName)}";
            string fileName = $"{saveName}";
            string fullPath = Path.Combine(saveFolder, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            imageFile.CopyTo(stream);

            return fileName;
        }
    }
}
