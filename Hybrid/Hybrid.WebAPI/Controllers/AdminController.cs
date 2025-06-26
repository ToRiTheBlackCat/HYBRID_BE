using Hybrid.Repositories.Models;
using Hybrid.Services.Services;
using Hybrid.Services.ViewModel.Accomplishment;
using Hybrid.Services.ViewModel.Course;
using Hybrid.Services.ViewModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Hybrid.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IUserService _userService;
        private readonly IMiniGameService _miniGameService;
        public AdminController(ICourseService courseService, IUserService userService, IMiniGameService miniGameService)
        {
            _courseService = courseService;
            _userService = userService;
            _miniGameService = miniGameService;
        }
        [HttpGet("analyze-courses")]
        public async Task<ActionResult<AnalyzeCourseResponse>> GetAnalyzeCourse()
        {
            var result = await _courseService.AnalyzeCourse();
            return Ok(result);
        }

        [HttpGet("analyze-users")]
        public async Task<ActionResult<AnalyzeUserResponse>> GetAnalyzeUser()
        {
            var result = await _userService.AnalyzeUser();
            return Ok(result);
        }

        [HttpGet("analyze-minigames")]
        public async Task<ActionResult<AnalyzeUserResponse>> GetAnalyzeMinigames()
        {
            var result = await _miniGameService.AnalyzeMinigames();
            return Ok(result);
        }
        [HttpPost("filter-user")]
        public async Task<ActionResult<List<FilterUsersResponse>>> FilterUserByDate(DateTime? fromDate, DateTime? toDate)
        {
            var userList = await _userService.FilterUsersByCreatedDate(fromDate, toDate);   

            return Ok(userList);
        }
    }
}
