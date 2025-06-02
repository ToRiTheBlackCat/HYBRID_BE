using Hybrid.Services.Services;
using Hybrid.Services.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hybrid.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [HttpGet]
        public async Task<ActionResult<List<CoursesResponse>>> GetAllCourse(string? courseName, string? levelId, int currentPage)
        {
            int pageSize = 12;
            var coursesList = await _courseService.GetAllCourses(courseName, levelId, currentPage, pageSize);
            return Ok(coursesList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetailCoursesResponse>> GetCourseById(string? courseId)
        {
            var foundCourse = await _courseService.GetCourseById(courseId);
            return Ok(foundCourse);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCourse([FromBody] CreateCourseRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (isSuccess, message) = await _courseService.CreateCourse(request);

            return Ok(new
            {
                isSuccess,
                message
            });

        }
    }
}
