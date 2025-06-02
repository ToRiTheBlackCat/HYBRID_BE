using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel
{
    public class CreateCourseRequest
    {
        [Required]
        public string CourseName { get; set; }
        [Required]
        public string DataText { get; set; }
        [Required]
        public string LevelId { get; set; }
    }
}
