using Azure;
using Hybrid.Services.Services;
using Hybrid.Services.ViewModel;
using Hybrid.Services.ViewModel.Accomplishment;
using Hybrid.Services.ViewModel.Minigames;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sprache;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hybrid.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiniGameController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IMiniGameService _miniGameService;
        private readonly IUserService _userService;
        public MiniGameController(IWebHostEnvironment env, IMiniGameService miniGameService, IUserService userService)
        {
            _env = env;
            _miniGameService = miniGameService;
            _userService = userService;
        }


        /// <summary>
        /// API_Get All Minigame Templates
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpGet("templates")]
        public async Task<ActionResult<List<GetMinigameTemplatesModel>>> GetMinigametTemplates()
        {
            var response = await _miniGameService.GetMinigameTemplatesAsync();

            return Ok(response);
        }

        [HttpGet("top")]
        public async Task<ActionResult> GetTopMinigames([FromQuery] int count)
        {
            var response = await _miniGameService.GetTopMinigamesAsync(count);

            return Ok(response);
        }

        /// <summary>
        /// API_Get MiniGame of Course
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<GetAllMinigameResponse>> GetMiniGameOfCourse(string courseId, [FromQuery] GetAllMinigameRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _miniGameService.GetMinigameOfCourseAsync(courseId, request);
            if (response == null)
            {
                return NotFound("Minigame not found.");
            }

            return Ok(response);
        }

        /// <summary>
        /// API_Get MiniGame of Teacher
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpGet("teacher/{teacherId}")]
        public async Task<ActionResult<GetAllMinigameResponse>> GetMiniGameOfTeacher(string teacherId, [FromQuery] GetAllMinigameRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _miniGameService.GetMinigameOfTeacherAsync(teacherId, request);
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

        #region Add Minigame Seperate usages
        /// <summary>
        /// API_Add Conjunction Minigame
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("conjunction")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<AddMiniGameResponse>> AddConjunction([FromForm] AddMiniGameRequest<ConjunctionQuestion> request)
        {
            return await AddMiniGame(request);
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
        public async Task<ActionResult<AddMiniGameResponse>> AddQuiz([FromForm] AddMiniGameRequest<QuizQuestion> request)
        {
            return await AddMiniGame(request);
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
        public async Task<ActionResult<AddMiniGameResponse>> AddAnagram([FromForm] AddMiniGameRequest<AnagramQuestion> request)
        {
            return await AddMiniGame(request);
        }

        /// <summary>
        /// API_Add RandomCard Minigame
        /// Created By: TuanCA
        /// Created Date: 08/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("random-card")]
        public async Task<ActionResult<AddMiniGameResponse>> AddRandomCard([FromForm] AddMiniGameRequest<RandomCardQuestion> request)
        {
            return await AddMiniGame(request);
        }

        /// <summary>
        /// API_Add Spelling Minigame
        /// Created By: TuanCA
        /// Created Date: 10/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("spelling")]
        public async Task<ActionResult<AddMiniGameResponse>> AddSpelling([FromForm] AddMiniGameRequest<SpellingQuestion> request)
        {
            return await AddMiniGame(request);
        }

        /// <summary>
        /// API_Add FlashCard Minigame
        /// Created By: TuanCA
        /// Created Date: 16/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("flash-card")]
        public async Task<ActionResult<AddMiniGameResponse>> AddFlashCard([FromForm] AddMiniGameRequest<FlashCardQuestion> request)
        {
            return await AddMiniGame(request);
        }

        /// <summary>
        /// API_Add Completion Minigame
        /// Created By: TuanCA
        /// Created Date: 18/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("completion")]
        public async Task<ActionResult<AddMiniGameResponse>> AddCompletion([FromForm] AddMiniGameRequest<CompletionQuestion> request)
        {
            return await AddMiniGame(request);
        }

        /// <summary>
        /// API_Add Pairing Minigame
        /// Created By: TuanCA
        /// Created Date: 18/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost("pairing")]
        public async Task<ActionResult<AddMiniGameResponse>> AddPairing([FromForm] AddMiniGameRequest<PairingQuestion> request)
        {
            return await AddMiniGame(request);
        }
        #endregion

        /// <summary>
        /// Generic method to handle adding minigames
        /// </summary>
        private async Task<ActionResult<AddMiniGameResponse>> AddMiniGame<T>([FromForm] AddMiniGameRequest<T> request) where T : MinigameModels
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate ImageFile
            if (IsValidImageFile(request.ImageFile))
            {
                if (request.GameData is IEnumerable<IMinigameWithPicture> gameData)
                {
                    foreach (var item in gameData)
                    {
                        if (item.Image != null && !IsValidImageFile(item.Image))
                        {
                            return BadRequest("Only image files (JPG, PNG, GIF, WebP) are allowed.");
                        }
                    }
                }
            }
            else
            {
                return BadRequest("Only image files (JPG, PNG, GIF, WebP) are allowed.");
            }

            if (request.GameData == null || !request.GameData.Any())
            {
                ModelState.AddModelError(nameof(request.GameData), "The GameData field must have atleast one item.");
                return BadRequest(ModelState);
            }

            request.TemplateId = GetValidMinigameTemplateId(request.GameData[0]);

            var result = await _miniGameService.AddMiniGameAsync(request, Path.GetExtension(request.ImageFile.FileName));
            if (!result.IsSuccess)
            {
                return Ok(result);
            }

            var _ = SaveImage(request.ImageFile, result.Model!.ThumbnailImage);

            // Save images for each minigame question
            if (request.GameData is IEnumerable<IMinigameWithPicture> minigames)
            {
                foreach (var item in minigames.Where(x => x.Image != null))
                {
                    SaveImage(item!.Image, item!.ImagePath);
                }
            }

            return Ok(result);
        }

        #region Update Minigame Seperate Usages
        /// <summary>
        /// API_Update Conjunction Minigame
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPut("conjunction")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<UpdateMinigameResponse>> UpdateConjunction([FromForm] UpdateMinigameRequest<ConjunctionQuestion> request, string fakeTeacherId = "")
        {
            return await UpdateMiniGame(request, fakeTeacherId);
        }

        /// <summary>
        /// API_Update Quiz Minigame
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPut("quiz")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<UpdateMinigameResponse>> UpdateQuiz([FromForm] UpdateMinigameRequest<QuizQuestion> request, string fakeTeacherId = "")
        {
            return await UpdateMiniGame(request, fakeTeacherId);
        }

        /// <summary>
        /// API_Update Anagram Minigame
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPut("anagram")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<UpdateMinigameResponse>> UpdateAnagram([FromForm] UpdateMinigameRequest<AnagramQuestion> request, string fakeTeacherId = "")
        {
            return await UpdateMiniGame(request, fakeTeacherId);
        }

        /// <summary>
        /// API_Update RandomCard Minigame
        /// Created By: TuanCA
        /// Created Date: 08/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPut("random-card")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<UpdateMinigameResponse>> UpdateRandomCard([FromForm] UpdateMinigameRequest<RandomCardQuestion> request, string fakeTeacherId = "")
        {
            return await UpdateMiniGame(request, fakeTeacherId);
        }

        /// <summary>
        /// API_Update Spelling Minigame
        /// Created By: TuanCA
        /// Created Date: 10/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPut("spelling")]
        public async Task<ActionResult<UpdateMinigameResponse>> UpdateSpelling([FromForm] UpdateMinigameRequest<SpellingQuestion> request, string fakeTeacherId = "")
        {
            return await UpdateMiniGame(request, fakeTeacherId);
        }

        /// <summary>
        /// API_Update FlashCard Minigame
        /// Created By: TuanCA
        /// Created Date: 16/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPut("flash-card")]
        public async Task<ActionResult<UpdateMinigameResponse>> UpdateFlashCard([FromForm] UpdateMinigameRequest<FlashCardQuestion> request, string fakeTeacherId = "")
        {
            return await UpdateMiniGame(request, fakeTeacherId);
        }

        /// <summary>
        /// API_Update Completion Minigame
        /// Created By: TuanCA
        /// Created Date: 18/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPut("completion")]
        public async Task<ActionResult<UpdateMinigameResponse>> UpdateCompletion([FromForm] UpdateMinigameRequest<CompletionQuestion> request, string fakeTeacherId = "")
        {
            return await UpdateMiniGame(request, fakeTeacherId);
        }

        /// <summary>
        /// API_Update Pairing Minigame
        /// Created By: TuanCA
        /// Created Date: 18/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPut("pairing")]
        public async Task<ActionResult<UpdateMinigameResponse>> UpdatePairing([FromForm] UpdateMinigameRequest<PairingQuestion> request, string fakeTeacherId = "")
        {
            return await UpdateMiniGame(request, fakeTeacherId);
        }
        #endregion

        /// <summary>
        /// Generic method to handle updating minigames
        /// </summary>
        /// <param name="fakeTeacherId">TeacherId for testing without Authentication</param>
        /// <returns></returns>
        private async Task<ActionResult<UpdateMinigameResponse>> UpdateMiniGame<T>(UpdateMinigameRequest<T> request, string fakeTeacherId = "") where T : MinigameModels
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

            // Validate ImageFile
            if (IsValidImageFile(request.ImageFile))
            {
                if (request.GameData is IEnumerable<IMinigameWithPicture> gameData)
                {
                    foreach (var item in gameData.Where(x => x.Image != null))
                    {
                        if (!IsValidImageFile(item.Image!))
                        {
                            return BadRequest("Only image files (JPG, PNG, GIF, WebP) are allowed.");
                        }
                    }
                }
            }
            else
            {
                return BadRequest("Only image files (JPG, PNG, GIF, WebP) are allowed.");
            }

            if (!request.GameData.Any())
            {
                ModelState.AddModelError(nameof(request.GameData), "The GameData field must have at least one item.");
                return BadRequest(ModelState);
            }

            request.TemplateId = GetValidMinigameTemplateId(request.GameData[0]);

            var result = await _miniGameService.UpdateMiniGameAsync(request, Path.GetExtension(request.ImageFile.FileName));
            if (!result.IsSuccess)
            {
                return Ok(result);
            }
            var _ = SaveImage(request.ImageFile, result.Model!.ThumbnailImage);

            // Save images for each minigame question
            if (request.GameData is IEnumerable<IMinigameWithPicture> minigames)
            {
                // Delete old images before saving new ones
                DeleteImagesById(result.Model.MinigameId.Trim());

                foreach (var item in minigames.Where(x => x.Image != null))
                {
                    SaveImage(item.Image!, item!.ImagePath);
                }
            }

            return Ok(result);
        }

        /// <summary>
        /// API_Delete Minigame
        /// Created By: TuanCA
        /// Created Date: 28/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpDelete]
        public async Task<ActionResult<DeleteMinigameResponse>> DeleteMinigame([Required] string minigameId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _miniGameService.DeleteMiniGameAsync(minigameId);

            if (result.IsSuccess)
            {
                // Path to the image of minigame
                string filePath = Path.Combine(_env.WebRootPath, "images", result.Minigame.ThumbnailImage);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Delete old images associated with the minigame
                DeleteImagesById(result.Minigame.MinigameId.Trim());
            }

            return Ok(result);
        }

        /// <summary>
        /// Saves the uploaded image file to the server
        /// Created By: TuanCA
        /// Created Date: 10/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        /// <param name="imageFile">The image file</param>
        /// <param name="savedName">The new name that the file will be saved under</param>
        /// <returns></returns>
        private string SaveImage(IFormFile imageFile, string savedName)
        {
            string saveFolder = Path.Combine(_env.WebRootPath, "images", "users");
            Directory.CreateDirectory(saveFolder); // Ensure the folder exists

            string filePath = Path.Combine(_env.WebRootPath, "images");
            string fileName = $"{savedName}";
            string fullPath = Path.Combine(filePath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            imageFile.CopyTo(stream);

            return fileName;
        }

        /// <summary>
        /// FUNC - Delete old images associated with the Minigame that match minigameId
        /// Created By: TuanCA
        /// Created Date: 10/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        /// <param name="minigameId">The ID of the deleted Minigame</param>
        private void DeleteImagesById(string minigameId)
        {
            string folderPath = Path.Combine(_env.WebRootPath, "images", "users");
            if (!Directory.Exists(folderPath))
                return;

            // Regex pattern: matches filenames like "MG123_img1.jpg", "MG123_img2.png", etc.
            string pattern = $@"^{minigameId}_img\d+\.\w+$";

            Regex regex = new Regex(pattern);

            foreach (string filePath in Directory.GetFiles(folderPath))
            {
                string fileName = Path.GetFileName(filePath);

                if (regex.IsMatch(fileName))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        /// <summary>
        /// Validates if the uploaded file is a valid image file
        /// Created By: TuanCA
        /// Created Date: 10/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns></returns>
        private bool IsValidImageFile(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return false;

            var permittedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
            if (permittedMimeTypes.Contains(imageFile.ContentType.ToLower()))
            {
                var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                if (validExtensions.Contains(extension))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get the TemplateId based on the type of minigame
        /// </summary>
        private string GetValidMinigameTemplateId(MinigameModels minigame)
        {
            string templateId = "";

            switch (minigame)
            {
                case ConjunctionQuestion conjunction:
                    templateId = "TP1";
                    break;
                case QuizQuestion quiz:
                    templateId = "TP2";
                    break;
                case AnagramQuestion anagram:
                    templateId = "TP3";
                    break;
                case RandomCardQuestion randomCard:
                    templateId = "TP4";
                    break;
                case SpellingQuestion spelling:
                    templateId = "TP5";
                    break;
                case FlashCardQuestion flashCard:
                    templateId = "TP6";
                    break;
                case CompletionQuestion completion:
                    templateId = "TP7";
                    break;
                case PairingQuestion pairing:
                    templateId = "TP8";
                    break;
                //case QuizQuestion quiz:
                //    templateId = "TP9";
                //    break;
                //case QuizQuestion quiz:
                //    templateId = "TP10";
                //    break;
                //case QuizQuestion quiz:
                //    templateId = "TP11";
                //    break;
                //case QuizQuestion quiz:
                //    templateId = "TP12";
                //    break;
                default:
                    throw new ArgumentException("Invalid minigame type", nameof(minigame));
            }

            return templateId;
        }
    }
}
