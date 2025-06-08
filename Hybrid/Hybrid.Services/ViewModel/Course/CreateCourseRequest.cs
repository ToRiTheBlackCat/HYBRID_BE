using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Course
{
    public class CreateCourseRequest
    {
        [Required]
        public string CourseName { get; set; }
        public string DataText { get; set; } = string.Empty;
        [Required]
        public string LevelId { get; set; }
        [Required]
        public string LevelName { get; set; }

        [Required]
        public List<Image> ImagesList { get; set; }
    }
    public class Image
    {
        public string imageString { get; set; }
    }
}
