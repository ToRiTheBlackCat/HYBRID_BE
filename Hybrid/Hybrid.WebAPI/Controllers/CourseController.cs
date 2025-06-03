using Hybrid.Services.Services;
using Hybrid.Services.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Hybrid.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IWebHostEnvironment _env;
        public CourseController(ICourseService courseService, IWebHostEnvironment env)
        {
            _courseService = courseService;
            _env = env;
        }

        /// <summary>
        /// API_GetAllCourse
        /// courseName- string / levelId - string / currentPage - int
        /// List of CoursesResponse
        /// Created By: TriNHM
        /// Created Date: 2/6/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<CoursesResponse>>> GetAllCourse(string? courseName, string? levelId, int currentPage)
        {
            int pageSize = 12;
            var coursesList = await _courseService.GetAllCourses(courseName, levelId, currentPage, pageSize);
            return Ok(coursesList);
        }

        /// <summary>
        /// API_GetCourseById
        /// id - string?
        /// DetailCoursesResponse_ViewModel
        /// Created By: TriNHM
        /// Created Date: 2/6/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<DetailCoursesResponse>> GetCourseById(string? id)
        {
            var foundCourse = await _courseService.GetCourseById(id);
            return Ok(foundCourse);
        }

        /// <summary>
        /// API_CreateCourse
        /// CreateCourseRequest_ViewModel
        /// isSuccess / Message
        /// Created By: TriNHM
        /// Created Date: 2/6/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> CreateCourse([FromBody] CreateCourseRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.DataText = SupportCreateDataText(request);

            var (isSuccess, message) = await _courseService.CreateCourse(request);

            return Ok(new
            {
                isSuccess,
                message
            });

        }

        private string SupportCreateDataText(CreateCourseRequest request)
        {
            if (request.ImagesList == null || request.ImagesList.Count == 0)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();
            builder.Append("<content>\n");

            //Add thumbnailImage 
            builder.Append($"\t<thumbnail>/courses/{request.LevelName}/{request.CourseName}/{request.ImagesList[0].imageString}<\thumbnail>\n");

            //Add other images
            builder.Append("\t<images>\n");
            for (int i = 1; i < request.ImagesList.Count; i++)
            {
                builder.Append($"\t\t<img>/courses/{request.LevelName}/{request.CourseName}/{request.ImagesList[i].imageString}</img>\n");
            }
            builder.Append("\t</images>\n");
            builder.Append("</content>");

            return builder.ToString();
        }


    }
}
