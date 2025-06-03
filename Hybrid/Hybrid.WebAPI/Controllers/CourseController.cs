using Hybrid.Services.Services;
using Hybrid.Services.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
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

        private string SupportCreateDataText<T>(T request) where T : class
        {
            var imagesListProp = typeof(T).GetProperty("ImagesList");
            var levelNameProp = typeof(T).GetProperty("LevelName");
            var courseNameProp = typeof(T).GetProperty("CourseName");

            var imagesList = imagesListProp?.GetValue(request) as IList;
            var levelName = levelNameProp?.GetValue(request)?.ToString();
            var courseName = courseNameProp?.GetValue(request)?.ToString();


            if (imagesList == null || imagesList.Count == 0) return string.Empty;


            var builder = new StringBuilder();
            builder.Append("<content>\n");

            //Add thumbnailImage 
            var imageStringProp = imagesList[0]?.GetType().GetProperty("imageString");
            var firstImageString = imageStringProp?.GetValue(imagesList[0])?.ToString();


            builder.Append($"\t<thumbnail>/courses/{levelName}/{courseName}/{firstImageString}</thumbnail>\n");

            //Add other images
            builder.Append("\t<images>\n");
            for (int i = 1; i < imagesList.Count; i++)
            {
                var imageObj = imagesList[i];
                var imageStr = imageStringProp?.GetValue(imageObj)?.ToString();

                builder.Append($"\t\t<img>/courses/{levelName}/{courseName}/{imageStr}</img>\n");
            }
            builder.Append("\t</images>\n");
            builder.Append("</content>");

            return builder.ToString();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCourse([FromBody] UpdateCourseRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var foundCourse = await _courseService.GetCourseById(request.CourseId);
            if (foundCourse == null)
            {
                return NotFound(ModelState);
            }

            request.DataText = SupportCreateDataText(request);
            var (isSuccess, message) = await _courseService.UpdateCourse(request);

            return Ok(new
            {
                isSuccess,
                message
            });
        }
    }
}
