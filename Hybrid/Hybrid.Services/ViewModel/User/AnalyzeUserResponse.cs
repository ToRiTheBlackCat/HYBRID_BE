using Hybrid.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.ViewModel.User
{
    public class AnalyzeUserResponse
    {
        public int NumberOfStudents { get; set; }
        public List<Student> StudentsList { get; set; } = new List<Student>();
        public int NumbersOfTeacher { get; set; }
        public List<Teacher> TeachersList { get; set; } = new List<Teacher>();

    }
}
