using Hybrid.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.Course
{
    public class CoursesResponse
    {
        public string CourseId { get; set; }

        public string CourseName { get; set; }

        public string LevelId { get; set; }

        public string LevelName { get; set; }
    }
}
