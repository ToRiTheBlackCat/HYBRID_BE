using Azure;
using Hybrid.Services.Services;
using Hybrid.Services.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
            if (!ModelState.IsValid)
            {
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
        [HttpPost("matching")]
        public async Task<IActionResult> AddMiniGame(AddMiniGameRequest<string> request)
        {
            throw new NotImplementedException();
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
